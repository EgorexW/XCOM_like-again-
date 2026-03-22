//#define DEBUG_BOUNDS

#if UNITY_2018_2_OR_NEWER
using UnityEngine.Rendering;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class RuntimePreviewGenerator
{
    class CameraSetup
    {
        Vector3 position;
        Quaternion rotation;

        Color backgroundColor;
        bool orthographic;
        float orthographicSize;
        float nearClipPlane;
        float farClipPlane;
        float aspect;
        int cullingMask;
        CameraClearFlags clearFlags;

        RenderTexture targetTexture;

        public void GetSetup(Camera camera)
        {
            position = camera.transform.position;
            rotation = camera.transform.rotation;

            backgroundColor = camera.backgroundColor;
            orthographic = camera.orthographic;
            orthographicSize = camera.orthographicSize;
            nearClipPlane = camera.nearClipPlane;
            farClipPlane = camera.farClipPlane;
            aspect = camera.aspect;
            cullingMask = camera.cullingMask;
            clearFlags = camera.clearFlags;

            targetTexture = camera.targetTexture;
        }

        public void ApplySetup(Camera camera)
        {
            camera.transform.position = position;
            camera.transform.rotation = rotation;

            camera.backgroundColor = backgroundColor;
            camera.orthographic = orthographic;
            camera.orthographicSize = orthographicSize;
            camera.aspect = aspect;
            camera.cullingMask = cullingMask;
            camera.clearFlags = clearFlags;

            // Assigning order or nearClipPlane and farClipPlane may matter because Unity clamps near to far and far to near
            if (nearClipPlane < camera.farClipPlane){
                camera.nearClipPlane = nearClipPlane;
                camera.farClipPlane = farClipPlane;
            }
            else{
                camera.farClipPlane = farClipPlane;
                camera.nearClipPlane = nearClipPlane;
            }

            camera.targetTexture = targetTexture;
            targetTexture = null;
        }
    }

    const int PREVIEW_LAYER = 22;
    static readonly Vector3 PREVIEW_POSITION = new(-250f, -250f, -250f);

    static Camera renderCamera;
    static readonly CameraSetup cameraSetup = new();

    static readonly Vector3[] boundingBoxPoints = new Vector3[8];
    static readonly Vector3[] localBoundsMinMax = new Vector3[2];

    static readonly List<Renderer> renderersList = new(64);
    static readonly List<int> layersList = new(64);

#if DEBUG_BOUNDS
	private static Material boundsDebugMaterial;
#endif

    static Camera m_internalCamera;
    static Camera InternalCamera{
        get{
            if (m_internalCamera == null){
                m_internalCamera = new GameObject("ModelPreviewGeneratorCamera").AddComponent<Camera>();
                m_internalCamera.enabled = false;
                m_internalCamera.nearClipPlane = 0.01f;
                m_internalCamera.cullingMask = 1 << PREVIEW_LAYER;
                m_internalCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
            }

            return m_internalCamera;
        }
    }

    public static Camera PreviewRenderCamera{ get; set; }

    static Vector3 m_previewDirection = new(-0.57735f, -0.57735f, -0.57735f); // Normalized (-1,-1,-1)
    public static Vector3 PreviewDirection{
        get => m_previewDirection;
        set => m_previewDirection = value.normalized;
    }

    static float m_padding;
    public static float Padding{
        get => m_padding;
        set => m_padding = Mathf.Clamp(value, -0.25f, 0.25f);
    }

    static Color m_backgroundColor = new(0.3f, 0.3f, 0.3f, 1f);
    public static Color BackgroundColor{
        get => m_backgroundColor;
        set => m_backgroundColor = value;
    }

    public static bool OrthographicMode{ get; set; }

    public static bool UseLocalBounds{ get; set; }

    static float m_renderSupersampling = 1f;
    public static float RenderSupersampling{
        get => m_renderSupersampling;
        set => m_renderSupersampling = Mathf.Max(value, 0.1f);
    }

    public static bool MarkTextureNonReadable{ get; set; } = true;

    public static Texture2D GenerateMaterialPreview(Material material, PrimitiveType previewPrimitive, int width = 64,
        int height = 64)
    {
        return GenerateMaterialPreviewInternal(material, previewPrimitive, null, null, width, height);
    }

    public static Texture2D GenerateMaterialPreviewWithShader(Material material, PrimitiveType previewPrimitive,
        Shader shader, string replacementTag, int width = 64, int height = 64)
    {
        return GenerateMaterialPreviewInternal(material, previewPrimitive, shader, replacementTag, width, height);
    }

#if UNITY_2018_2_OR_NEWER
    public static void GenerateMaterialPreviewAsync(Action<Texture2D> callback, Material material,
        PrimitiveType previewPrimitive, int width = 64, int height = 64)
    {
        GenerateMaterialPreviewInternal(material, previewPrimitive, null, null, width, height, callback);
    }

    public static void GenerateMaterialPreviewWithShaderAsync(Action<Texture2D> callback, Material material,
        PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width = 64, int height = 64)
    {
        GenerateMaterialPreviewInternal(material, previewPrimitive, shader, replacementTag, width, height, callback);
    }
#endif

#if UNITY_2018_2_OR_NEWER
    static Texture2D GenerateMaterialPreviewInternal(Material material, PrimitiveType previewPrimitive, Shader shader,
        string replacementTag, int width, int height, Action<Texture2D> asyncCallback = null)
#else
	private static Texture2D GenerateMaterialPreviewInternal( Material material, PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width, int height )
#endif
    {
        var previewModel = GameObject.CreatePrimitive(previewPrimitive);
        previewModel.gameObject.hideFlags = HideFlags.HideAndDontSave;
        previewModel.GetComponent<Renderer>().sharedMaterial = material;

        try{
#if UNITY_2018_2_OR_NEWER
            return GenerateModelPreviewInternal(previewModel.transform, shader, replacementTag, width, height, false,
                true, asyncCallback);
#else
			return GenerateModelPreviewInternal( previewModel.transform, shader, replacementTag, width, height, false, true );
#endif
        }
        catch (Exception e){
            Debug.LogException(e);
        }
        finally{
            Object.DestroyImmediate(previewModel);
        }

        return null;
    }

    public static Texture2D GenerateModelPreview(Transform model, int width = 64, int height = 64,
        bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
    {
        return GenerateModelPreviewInternal(model, null, null, width, height, shouldCloneModel,
            shouldIgnoreParticleSystems);
    }

    public static Texture2D GenerateModelPreviewWithShader(Transform model, Shader shader, string replacementTag,
        int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
    {
        return GenerateModelPreviewInternal(model, shader, replacementTag, width, height, shouldCloneModel,
            shouldIgnoreParticleSystems);
    }

#if UNITY_2018_2_OR_NEWER
    public static void GenerateModelPreviewAsync(Action<Texture2D> callback, Transform model, int width = 64,
        int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
    {
        GenerateModelPreviewInternal(model, null, null, width, height, shouldCloneModel, shouldIgnoreParticleSystems,
            callback);
    }

    public static void GenerateModelPreviewWithShaderAsync(Action<Texture2D> callback, Transform model, Shader shader,
        string replacementTag, int width = 64, int height = 64, bool shouldCloneModel = false,
        bool shouldIgnoreParticleSystems = true)
    {
        GenerateModelPreviewInternal(model, shader, replacementTag, width, height, shouldCloneModel,
            shouldIgnoreParticleSystems, callback);
    }
#endif

#if UNITY_2018_2_OR_NEWER
    static Texture2D GenerateModelPreviewInternal(Transform model, Shader shader, string replacementTag, int width,
        int height, bool shouldCloneModel, bool shouldIgnoreParticleSystems, Action<Texture2D> asyncCallback = null)
#else
	private static Texture2D GenerateModelPreviewInternal( Transform model, Shader shader, string replacementTag, int width, int height, bool shouldCloneModel, bool shouldIgnoreParticleSystems )
#endif
    {
        if (!model){
#if UNITY_2018_2_OR_NEWER
            if (asyncCallback != null){
                asyncCallback(null);
            }
#endif

            return null;
        }

        Texture2D result = null;

        if (!model.gameObject.scene.IsValid() || !model.gameObject.scene.isLoaded){
            shouldCloneModel = true;
        }

        Transform previewObject;
        if (shouldCloneModel){
            previewObject = Object.Instantiate(model, null, false);
            previewObject.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
        else{
            previewObject = model;

            layersList.Clear();
            GetLayerRecursively(previewObject);
        }

        var isStatic = IsStatic(model);
        var wasActive = previewObject.gameObject.activeSelf;
        var prevPos = previewObject.position;
        var prevRot = previewObject.rotation;

#if UNITY_2018_2_OR_NEWER
        var asyncOperationStarted = false;
#endif

#if DEBUG_BOUNDS
		Transform boundsDebugCube = null;
#endif

        try{
            SetupCamera();
            SetLayerRecursively(previewObject);

            if (!isStatic){
                previewObject.position = PREVIEW_POSITION;
                previewObject.rotation = Quaternion.identity;
            }

            if (!wasActive){
                previewObject.gameObject.SetActive(true);
            }

            var cameraRotation = Quaternion.LookRotation(previewObject.rotation * m_previewDirection, previewObject.up);
            var previewBounds = new Bounds();
            if (!CalculateBounds(previewObject, shouldIgnoreParticleSystems, cameraRotation, out previewBounds)){
#if UNITY_2018_2_OR_NEWER
                if (asyncCallback != null){
                    asyncCallback(null);
                }
#endif

                return null;
            }

#if DEBUG_BOUNDS
			if( !boundsDebugMaterial )
			{
				boundsDebugMaterial = new Material( Shader.Find( "Sprites/Default" ) )
				{
					hideFlags = HideFlags.HideAndDontSave,
					color = new Color( 0.5f, 0.5f, 0.5f, 0.5f )
				};
			}

			boundsDebugCube = GameObject.CreatePrimitive( PrimitiveType.Cube ).transform;
			boundsDebugCube.localPosition = previewBounds.center;
			boundsDebugCube.localRotation = m_useLocalBounds ? cameraRotation : Quaternion.identity;
			boundsDebugCube.localScale = previewBounds.size;
			boundsDebugCube.gameObject.layer = PREVIEW_LAYER;
			boundsDebugCube.gameObject.hideFlags = HideFlags.HideAndDontSave;

			boundsDebugCube.GetComponent<Renderer>().sharedMaterial = boundsDebugMaterial;
#endif

            renderCamera.aspect = (float)width / height;
            renderCamera.transform.rotation = cameraRotation;

            CalculateCameraPosition(renderCamera, previewBounds, m_padding);

            renderCamera.farClipPlane = (renderCamera.transform.position - previewBounds.center).magnitude +
                                        (UseLocalBounds
                                            ? previewBounds.extents.z * 1.01f
                                            : previewBounds.size.magnitude);

            var activeRT = RenderTexture.active;
            RenderTexture renderTexture = null;
            try{
                var supersampledWidth = Mathf.RoundToInt(width * m_renderSupersampling);
                var supersampledHeight = Mathf.RoundToInt(height * m_renderSupersampling);

                renderTexture = RenderTexture.GetTemporary(supersampledWidth, supersampledHeight, 16);
                RenderTexture.active = renderTexture;
                if (m_backgroundColor.a < 1f){
                    GL.Clear(true, true, m_backgroundColor);
                }

                renderCamera.targetTexture = renderTexture;

                if (!shader){
                    renderCamera.Render();
                }
                else{
                    renderCamera.RenderWithShader(shader, replacementTag ?? string.Empty);
                }

                renderCamera.targetTexture = null;

                if (supersampledWidth != width || supersampledHeight != height){
                    RenderTexture _renderTexture = null;
                    try{
                        _renderTexture = RenderTexture.GetTemporary(width, height, 16);
                        RenderTexture.active = _renderTexture;
                        if (m_backgroundColor.a < 1f){
                            GL.Clear(true, true, m_backgroundColor);
                        }

                        Graphics.Blit(renderTexture, _renderTexture);
                    }
                    finally{
                        if (_renderTexture){
                            RenderTexture.ReleaseTemporary(renderTexture);
                            renderTexture = _renderTexture;
                        }
                    }
                }

#if UNITY_2018_2_OR_NEWER
                if (asyncCallback != null){
                    AsyncGPUReadback.Request(renderTexture, 0,
                        m_backgroundColor.a < 1f ? TextureFormat.RGBA32 : TextureFormat.RGB24, asyncResult =>
                        {
                            try{
                                result = new Texture2D(width, height,
                                    m_backgroundColor.a < 1f ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);
                                if (!asyncResult.hasError){
                                    result.LoadRawTextureData(asyncResult.GetData<byte>());
                                }
                                else{
                                    Debug.LogWarning(
                                        "Async thumbnail request failed, falling back to conventional method");

                                    var _activeRT = RenderTexture.active;
                                    try{
                                        RenderTexture.active = renderTexture;
                                        result.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, false);
                                    }
                                    finally{
                                        RenderTexture.active = _activeRT;
                                    }
                                }

                                result.Apply(false, MarkTextureNonReadable);
                                asyncCallback(result);
                            }
                            finally{
                                if (renderTexture){
                                    RenderTexture.ReleaseTemporary(renderTexture);
                                }
                            }
                        });

                    asyncOperationStarted = true;
                }
                else
#endif
                {
                    result = new Texture2D(width, height,
                        m_backgroundColor.a < 1f ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);
                    result.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, false);
                    result.Apply(false, MarkTextureNonReadable);
                }
            }
            finally{
                RenderTexture.active = activeRT;

                if (renderTexture){
#if UNITY_2018_2_OR_NEWER
                    if (!asyncOperationStarted)
#endif
                    {
                        RenderTexture.ReleaseTemporary(renderTexture);
                    }
                }
            }
        }
        catch (Exception e){
            Debug.LogException(e);
        }
        finally{
#if DEBUG_BOUNDS
			if( boundsDebugCube )
				Object.DestroyImmediate( boundsDebugCube.gameObject );
#endif

            if (shouldCloneModel){
                Object.DestroyImmediate(previewObject.gameObject);
            }
            else{
                if (!wasActive){
                    previewObject.gameObject.SetActive(false);
                }

                if (!isStatic){
                    previewObject.position = prevPos;
                    previewObject.rotation = prevRot;
                }

                var index = 0;
                SetLayerRecursively(previewObject, ref index);
            }

            if (renderCamera == PreviewRenderCamera){
                cameraSetup.ApplySetup(renderCamera);
            }
        }

#if UNITY_2018_2_OR_NEWER
        if (!asyncOperationStarted && asyncCallback != null){
            asyncCallback(null);
        }
#endif

        return result;
    }

    // Calculates AABB bounds of the target object (AABB will include its child objects)
    public static bool CalculateBounds(Transform target, bool shouldIgnoreParticleSystems, Quaternion cameraRotation,
        out Bounds bounds)
    {
        renderersList.Clear();
        target.GetComponentsInChildren(renderersList);

        var inverseCameraRotation = Quaternion.Inverse(cameraRotation);
        var localBoundsMin = new Vector3(float.MaxValue - 1f, float.MaxValue - 1f, float.MaxValue - 1f);
        var localBoundsMax = new Vector3(float.MinValue + 1f, float.MinValue + 1f, float.MinValue + 1f);

        bounds = new Bounds();
        var hasBounds = false;
        for (var i = 0; i < renderersList.Count; i++){
            if (!renderersList[i].enabled){
                continue;
            }

            if (shouldIgnoreParticleSystems && renderersList[i] is ParticleSystemRenderer){
                continue;
            }

            // Local bounds calculation code taken from: https://github.com/Unity-Technologies/UnityCsReference/blob/0355e09029fa1212b7f2e821f41565df8e8814c7/Editor/Mono/InternalEditorUtility.bindings.cs#L710
            if (UseLocalBounds){
#if UNITY_2021_2_OR_NEWER
                var localBounds = renderersList[i].localBounds;
#else
				MeshFilter meshFilter = renderersList[i].GetComponent<MeshFilter>();
				if( !meshFilter || !meshFilter.sharedMesh )
					continue;

				Bounds localBounds = meshFilter.sharedMesh.bounds;
#endif

                var transform = renderersList[i].transform;
                localBoundsMinMax[0] = localBounds.min;
                localBoundsMinMax[1] = localBounds.max;

                for (var x = 0; x < 2; x++)
                for (var y = 0; y < 2; y++)
                for (var z = 0; z < 2; z++){
                    var point = inverseCameraRotation * transform.TransformPoint(new Vector3(localBoundsMinMax[x].x,
                        localBoundsMinMax[y].y, localBoundsMinMax[z].z));
                    localBoundsMin = Vector3.Min(localBoundsMin, point);
                    localBoundsMax = Vector3.Max(localBoundsMax, point);
                }

                hasBounds = true;
            }
            else if (!hasBounds){
                bounds = renderersList[i].bounds;
                hasBounds = true;
            }
            else{
                bounds.Encapsulate(renderersList[i].bounds);
            }
        }

        if (UseLocalBounds && hasBounds){
            bounds = new Bounds(cameraRotation * ((localBoundsMin + localBoundsMax) * 0.5f),
                localBoundsMax - localBoundsMin);
        }

        return hasBounds;
    }

    // Moves camera in a way such that it will encapsulate bounds perfectly
    public static void CalculateCameraPosition(Camera camera, Bounds bounds, float padding = 0f)
    {
        var cameraTR = camera.transform;

        var cameraDirection = cameraTR.forward;
        var aspect = camera.aspect;

        if (padding != 0f){
            bounds.size *= 1f + padding * 2f; // Padding applied to both edges, hence multiplied by 2
        }

        var boundsCenter = bounds.center;
        var boundsExtents = bounds.extents;
        var boundsSize = 2f * boundsExtents;

        // Calculate corner points of the Bounds
        if (UseLocalBounds){
            var localBoundsMatrix = Matrix4x4.TRS(boundsCenter, camera.transform.rotation, Vector3.one);
            var point = boundsExtents;
            boundingBoxPoints[0] = localBoundsMatrix.MultiplyPoint3x4(point);
            point.x -= boundsSize.x;
            boundingBoxPoints[1] = localBoundsMatrix.MultiplyPoint3x4(point);
            point.y -= boundsSize.y;
            boundingBoxPoints[2] = localBoundsMatrix.MultiplyPoint3x4(point);
            point.x += boundsSize.x;
            boundingBoxPoints[3] = localBoundsMatrix.MultiplyPoint3x4(point);
            point.z -= boundsSize.z;
            boundingBoxPoints[4] = localBoundsMatrix.MultiplyPoint3x4(point);
            point.x -= boundsSize.x;
            boundingBoxPoints[5] = localBoundsMatrix.MultiplyPoint3x4(point);
            point.y += boundsSize.y;
            boundingBoxPoints[6] = localBoundsMatrix.MultiplyPoint3x4(point);
            point.x += boundsSize.x;
            boundingBoxPoints[7] = localBoundsMatrix.MultiplyPoint3x4(point);
        }
        else{
            var point = boundsCenter + boundsExtents;
            boundingBoxPoints[0] = point;
            point.x -= boundsSize.x;
            boundingBoxPoints[1] = point;
            point.y -= boundsSize.y;
            boundingBoxPoints[2] = point;
            point.x += boundsSize.x;
            boundingBoxPoints[3] = point;
            point.z -= boundsSize.z;
            boundingBoxPoints[4] = point;
            point.x -= boundsSize.x;
            boundingBoxPoints[5] = point;
            point.y += boundsSize.y;
            boundingBoxPoints[6] = point;
            point.x += boundsSize.x;
            boundingBoxPoints[7] = point;
        }

        if (camera.orthographic){
            cameraTR.position = boundsCenter;

            float minX = float.PositiveInfinity, minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;

            for (var i = 0; i < boundingBoxPoints.Length; i++){
                var localPoint = cameraTR.InverseTransformPoint(boundingBoxPoints[i]);
                if (localPoint.x < minX){
                    minX = localPoint.x;
                }
                if (localPoint.x > maxX){
                    maxX = localPoint.x;
                }
                if (localPoint.y < minY){
                    minY = localPoint.y;
                }
                if (localPoint.y > maxY){
                    maxY = localPoint.y;
                }
            }

            var distance = boundsExtents.magnitude + 1f;
            camera.orthographicSize = Mathf.Max(maxY - minY, (maxX - minX) / aspect) * 0.5f;
            cameraTR.position = boundsCenter - cameraDirection * distance;
        }
        else{
            Vector3 cameraUp = cameraTR.up, cameraRight = cameraTR.right;

            var verticalFOV = camera.fieldOfView * 0.5f;
            var horizontalFOV = Mathf.Atan(Mathf.Tan(verticalFOV * Mathf.Deg2Rad) * aspect) * Mathf.Rad2Deg;

            // Normals of the camera's frustum planes
            var topFrustumPlaneNormal = Quaternion.AngleAxis(90f + verticalFOV, -cameraRight) * cameraDirection;
            var bottomFrustumPlaneNormal = Quaternion.AngleAxis(90f + verticalFOV, cameraRight) * cameraDirection;
            var rightFrustumPlaneNormal = Quaternion.AngleAxis(90f + horizontalFOV, cameraUp) * cameraDirection;
            var leftFrustumPlaneNormal = Quaternion.AngleAxis(90f + horizontalFOV, -cameraUp) * cameraDirection;

            // Credit for algorithm: https://stackoverflow.com/a/66113254/2373034
            // 1. Find edge points of the bounds using the camera's frustum planes
            // 2. Create a plane for each edge point that goes through the point and has the corresponding frustum plane's normal
            // 3. Find the intersection line of horizontal edge points' planes (horizontalIntersection) and vertical edge points' planes (verticalIntersection)
            //    If we move the camera along horizontalIntersection, the bounds will always with the camera's width perfectly (similar effect goes for verticalIntersection)
            // 4. Find the closest line segment between these two lines (horizontalIntersection and verticalIntersection) and place the camera at the farthest point on that line
            int leftmostPoint = -1, rightmostPoint = -1, topmostPoint = -1, bottommostPoint = -1;
            for (var i = 0; i < boundingBoxPoints.Length; i++){
                if (leftmostPoint < 0 && IsOutermostPointInDirection(i, leftFrustumPlaneNormal)){
                    leftmostPoint = i;
                }
                if (rightmostPoint < 0 && IsOutermostPointInDirection(i, rightFrustumPlaneNormal)){
                    rightmostPoint = i;
                }
                if (topmostPoint < 0 && IsOutermostPointInDirection(i, topFrustumPlaneNormal)){
                    topmostPoint = i;
                }
                if (bottommostPoint < 0 && IsOutermostPointInDirection(i, bottomFrustumPlaneNormal)){
                    bottommostPoint = i;
                }
            }

            var horizontalIntersection =
                GetPlanesIntersection(new Plane(leftFrustumPlaneNormal, boundingBoxPoints[leftmostPoint]),
                    new Plane(rightFrustumPlaneNormal, boundingBoxPoints[rightmostPoint]));
            var verticalIntersection =
                GetPlanesIntersection(new Plane(topFrustumPlaneNormal, boundingBoxPoints[topmostPoint]),
                    new Plane(bottomFrustumPlaneNormal, boundingBoxPoints[bottommostPoint]));

            Vector3 closestPoint1, closestPoint2;
            FindClosestPointsOnTwoLines(horizontalIntersection, verticalIntersection, out closestPoint1,
                out closestPoint2);

            cameraTR.position = Vector3.Dot(closestPoint1 - closestPoint2, cameraDirection) < 0
                ? closestPoint1
                : closestPoint2;
        }
    }

    // Returns whether or not the given point is the outermost point in the given direction among all points of the bounds
    static bool IsOutermostPointInDirection(int pointIndex, Vector3 direction)
    {
        var point = boundingBoxPoints[pointIndex];
        for (var i = 0; i < boundingBoxPoints.Length; i++)
            if (i != pointIndex && Vector3.Dot(direction, boundingBoxPoints[i] - point) > 0){
                return false;
            }

        return true;
    }

    // Credit: https://stackoverflow.com/a/32410473/2373034
    // Returns the intersection line of the 2 planes
    static Ray GetPlanesIntersection(Plane p1, Plane p2)
    {
        var p3Normal = Vector3.Cross(p1.normal, p2.normal);
        var det = p3Normal.sqrMagnitude;

        return new Ray(
            (Vector3.Cross(p3Normal, p2.normal) * p1.distance + Vector3.Cross(p1.normal, p3Normal) * p2.distance) / det,
            p3Normal);
    }

    // Credit: http://wiki.unity3d.com/index.php/3d_Math_functions
    // Returns the edge points of the closest line segment between 2 lines
    static void FindClosestPointsOnTwoLines(Ray line1, Ray line2, out Vector3 closestPointLine1,
        out Vector3 closestPointLine2)
    {
        var line1Direction = line1.direction;
        var line2Direction = line2.direction;

        var a = Vector3.Dot(line1Direction, line1Direction);
        var b = Vector3.Dot(line1Direction, line2Direction);
        var e = Vector3.Dot(line2Direction, line2Direction);

        var d = a * e - b * b;

        var r = line1.origin - line2.origin;
        var c = Vector3.Dot(line1Direction, r);
        var f = Vector3.Dot(line2Direction, r);

        var s = (b * f - c * e) / d;
        var t = (a * f - c * b) / d;

        closestPointLine1 = line1.origin + line1Direction * s;
        closestPointLine2 = line2.origin + line2Direction * t;
    }

    static void SetupCamera()
    {
        if (PreviewRenderCamera){
            cameraSetup.GetSetup(PreviewRenderCamera);

            renderCamera = PreviewRenderCamera;
            renderCamera.nearClipPlane = 0.01f;
            renderCamera.cullingMask = 1 << PREVIEW_LAYER;
        }
        else{
            renderCamera = InternalCamera;
        }

        renderCamera.backgroundColor = m_backgroundColor;
        renderCamera.orthographic = OrthographicMode;
        renderCamera.clearFlags = m_backgroundColor.a < 1f ? CameraClearFlags.Depth : CameraClearFlags.Color;
    }

    static bool IsStatic(Transform obj)
    {
        if (obj.gameObject.isStatic){
            return true;
        }

        for (var i = 0; i < obj.childCount; i++)
            if (IsStatic(obj.GetChild(i))){
                return true;
            }

        return false;
    }

    static void SetLayerRecursively(Transform obj)
    {
        obj.gameObject.layer = PREVIEW_LAYER;
        for (var i = 0; i < obj.childCount; i++)
            SetLayerRecursively(obj.GetChild(i));
    }

    static void GetLayerRecursively(Transform obj)
    {
        layersList.Add(obj.gameObject.layer);
        for (var i = 0; i < obj.childCount; i++)
            GetLayerRecursively(obj.GetChild(i));
    }

    static void SetLayerRecursively(Transform obj, ref int index)
    {
        obj.gameObject.layer = layersList[index++];
        for (var i = 0; i < obj.childCount; i++)
            SetLayerRecursively(obj.GetChild(i), ref index);
    }
}