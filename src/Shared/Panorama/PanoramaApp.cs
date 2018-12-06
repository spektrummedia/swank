using System;
using Urho;
using Urho.Resources;
using Urho.Urho2D;

namespace Plugin.Swank.Panorama
{
    public class PanoramaApp : Application
    {
        private float _pitch, _yaw;
        public bool HasBeenIn360ModeAtLeastOnce { get; set; }
        public bool CurrentImageIs360 { get; set; }

        public PanoramaApp(ApplicationOptions options = null) : base(options)
        {
        }

        public void SetImage(MemoryBuffer imageBuff)
        {
            InvokeOnMain(() =>
            {
                if (imageBuff == null || imageBuff.Size <= 0)
                {
                    // Do not load empty images
                    return;
                }

                if (IsDeleted || IsClosed)
                {
                    // Do not try to load when application is stopped
                    return;
                }

                if (Renderer == null || Renderer.IsDeleted)
                {
                    // Do not load when renderer is deleted
                    return;
                }

                var sphere = Renderer.GetViewport(0).Scene.GetChild("room", false);
                if (sphere == null)
                {
                    return;
                }

                var image = new Image();
                var isLoaded = image.Load(imageBuff);
                if (!isLoaded)
                {
                    throw new Exception("This image cannot be load by Swank");
                }

                var texture = new Texture2D();
                var isTextureLoaded = texture.SetData(image, false);
                if (!isTextureLoaded)
                {
                    throw new Exception("This texture cannot be load by Swank");
                }

                sphere.GetComponent<StaticModel>().GetMaterial(0).SetTexture(TextureUnit.Diffuse, texture);
            });
        }

        public void SetFieldOfView(float fieldOfView)
        {
            Renderer.GetViewport(0).Camera.Fov = fieldOfView;
        }

        public void SetPitch(float pitch)
        {
            var camera = Renderer.GetViewport(0).Scene.GetChild("camera", false);

            if (camera != null)
            {
                _pitch = pitch;
                camera.Rotation = new Quaternion(pitch, _yaw, 0);
            }
        }

        public void SetYaw(float yaw)
        {
            var camera = Renderer.GetViewport(0).Scene.GetChild("camera", false);

            if (camera != null)
            {
                _yaw = yaw;
                camera.Rotation = new Quaternion(_pitch, _yaw, 0);
            }
        }

        protected override void Start()
        {
            base.Start();
            Create3DObject();
        }

        public void Create3DObject()
        {
            // Scene
            var scene = new Scene();
            scene.CreateComponent<Octree>();

            // Node (Rotation and Position)
            var node = scene.CreateChild("room");
            node.Position = new Vector3(0, 0, 0);
            //node.Rotation = new Quaternion(10, 60, 10);
            node.SetScale(1f);

            // Model
            var modelObject = node.CreateComponent<StaticModel>();
            modelObject.Model = ResourceCache.GetModel("Models/Sphere.mdl");

            var zoneNode = scene.CreateChild("Zone");
            var zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-300.0f, 300.0f));
            zone.AmbientColor = new Color(1f, 1f, 1f);

            var material = new Material();
            material.SetTechnique(0, CoreAssets.Techniques.DiffNormal, 0, 0);
            material.CullMode = CullMode.Cw;
            modelObject.SetMaterial(material);

            // Camera
            var cameraNode = scene.CreateChild("camera");
            var camera = cameraNode.CreateComponent<Camera>();
            camera.Fov = 75.8f;

            // Viewport
            Renderer.SetViewport(0, new Viewport(scene, camera, null));
        }
    }
}