using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotOriko.Utils {
    public static class GameObjectUtils {
        static public T FindInParents<T>(GameObject go) where T : Component {
            if (go == null) return null;
            T comp = go.GetComponent<T>();

            if (null == comp) {
                Transform t = go.transform.parent;

                while (t != null && comp == null) {
                    comp = t.gameObject.GetComponent<T>();
                    t = t.parent;
                }
            }
            return (T)comp;
        }

        public static void setAlpha(GameObject gameObject, float alpha) {
            if (!gameObject.activeSelf)
                return;

            foreach (Transform child in gameObject.transform) {
                setAlpha(child.gameObject, alpha);
            }

            Color color;

            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (null != spriteRenderer) {
                color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
                return;
            }

            UnityEngine.UI.Graphic graphic = gameObject.GetComponent<UnityEngine.UI.Graphic>();
            if (null != graphic) {
                color = graphic.color;
                color.a = alpha;
                graphic.color = color;
                return;
            }

            if (null != gameObject.renderer) {
                if (!gameObject.renderer.material.HasProperty("_Color"))
                    return;

                color = gameObject.renderer.material.color;
                color.a = alpha;
                gameObject.renderer.material.color = color;
                return;
            }

            if (null != gameObject.guiTexture) {
                color = gameObject.guiTexture.color;
                color.a = alpha;
                gameObject.guiTexture.color = color;
                return;
            }
        }

        public static void AlignZ(Transform parent) {
            foreach (Transform child in parent) {
                AlignZ(child);
            }
            parent.localPosition = parent.localPosition.ToVector2();
        }

        public static GameObject childByName(GameObject parent, string name) {
            foreach (Transform child in parent.transform) {
                if (child.gameObject.name.Equals(name))
                    return child.gameObject;
            }

            return null;
        }

        public static ArrayList getAnimationsNames(GameObject gameObject) {
            ArrayList names = new ArrayList();

            foreach (AnimationState animationState in gameObject.animation) {
                names.Add(animationState.name);
            }

            return names;
        }

        public static string getRandomAnimation(GameObject animation, string name) {
            ArrayList animationsNames = getAnimationsNames(animation);

            ArrayList randomAnimations = new ArrayList();

            for (int i = 0; i < animationsNames.Count; i++) {
                if (((string)animationsNames[i]).ToLower().Contains(name.ToLower())) {
                    randomAnimations.Add(animationsNames[i]);
                }
            }

            if (randomAnimations.Count == 0 && name == "Walk") {
                return getRandomAnimation(animation, "Fly");
            }

            return (string)randomAnimations[UnityEngine.Random.Range(0, randomAnimations.Count)];
        }

        public static bool intersects(BoxCollider firstCollider, BoxCollider secondCollider) {
            return intersectBox(firstCollider, secondCollider);
        }

        private static bool intersectBox(BoxCollider firstCollider, BoxCollider secondCollider) {
            Vector3 firstPosition = firstCollider.gameObject.transform.position;
            Vector3 secondPosition = secondCollider.gameObject.transform.position;

            Vector3 firstCenter = firstCollider.center;
            Vector3 secondCenter = secondCollider.center;

            Vector3 firstSize = new Vector3(firstCollider.size.x * firstCollider.transform.lossyScale.x,
                firstCollider.size.y * firstCollider.transform.lossyScale.y,
                firstCollider.size.z * firstCollider.transform.lossyScale.z);
            Vector3 secondSize = new Vector3(secondCollider.size.x * secondCollider.transform.lossyScale.x,
                secondCollider.size.y * secondCollider.transform.lossyScale.y,
                secondCollider.size.z * secondCollider.transform.lossyScale.z);

            return !(firstPosition.x + firstCenter.x - firstSize.x / 2 > secondPosition.x + secondCenter.x + secondSize.x / 2
                || firstPosition.x + firstCenter.x + firstSize.x / 2 < secondPosition.x + secondCenter.x - secondSize.x / 2)
                && !(firstPosition.y + firstCenter.y - firstSize.y / 2 > secondPosition.y + secondCenter.y + secondSize.y / 2
                || firstPosition.y + firstCenter.y + firstSize.y / 2 < secondPosition.y + secondCenter.y - secondSize.y / 2);
        }

        public static void setLayer(GameObject obj, int layer) {
            if (obj.layer != layer) {
                obj.layer = layer;
            }

            Transform cachedTransform = obj.transform;

            if (cachedTransform.childCount > 0) {
                for (int i = 0; i < cachedTransform.childCount; i++) {
                    setLayer(cachedTransform.GetChild(i).gameObject, layer);
                }
            }
        }

        public static void setOrderLayer(GameObject obj, int orderLayer) {
            List<Renderer> renders = new List<Renderer>();

            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>(true);
            if (meshes != null)
                renders.AddRange(meshes);

            SkinnedMeshRenderer[] skMeshes = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (skMeshes != null)
                renders.AddRange(skMeshes);

            ParticleRenderer[] particleRenderers = obj.GetComponentsInChildren<ParticleRenderer>(true);
            if (null != particleRenderers)
                renders.AddRange(particleRenderers);

            ParticleSystemRenderer[] particleSystemRenderers = obj.GetComponentsInChildren<ParticleSystemRenderer>(true);
            if (null != particleSystemRenderers)
                renders.AddRange(particleSystemRenderers);

            Renderer[] simpleRenders = obj.GetComponentsInChildren<Renderer>(true);
            if (null != simpleRenders)
                renders.AddRange(simpleRenders);

            meshes = obj.GetComponents<MeshRenderer>();
            if (meshes != null)
                renders.AddRange(meshes);

            skMeshes = obj.GetComponents<SkinnedMeshRenderer>();
            if (skMeshes != null)
                renders.AddRange(skMeshes);

            particleRenderers = obj.GetComponents<ParticleRenderer>();
            if (null != particleRenderers)
                renders.AddRange(particleRenderers);

            particleSystemRenderers = obj.GetComponents<ParticleSystemRenderer>();
            if (null != particleSystemRenderers)
                renders.AddRange(particleSystemRenderers);

            simpleRenders = obj.GetComponents<Renderer>();
            if (null != simpleRenders)
                renders.AddRange(simpleRenders);

            foreach (Renderer render in renders)
                render.sortingOrder = orderLayer;
        }

        public static void setLayer(Component obj, int layer) {
            obj.gameObject.layer = layer;

            Transform cachedTransform = obj.transform;

            if (cachedTransform.childCount > 0) {
                for (int i = 0; i < cachedTransform.childCount; i++) {
                    setLayer(cachedTransform.GetChild(i), layer);
                }
            }
        }

        public static void DestroyComponentInChilds<T>(Transform parent) where T : Component {
            Component[] components = parent.GetComponentsInChildren<T>();
            foreach (T component in components) {
                GameObject.Destroy(component);
            }
        }

        public static GameObject RecursiveChildByName(GameObject parent, string name, Boolean findInactive = false) {
            GameObject result = null;

            foreach (Transform child in parent.transform) {
                if (!findInactive && !child.gameObject.activeSelf)
                    continue;

                result = RecursiveChildByName(child.gameObject, name, findInactive);

                if (null != result)
                    return result;
            }

            return (parent.name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ? parent : result;
        }

        public static Transform RecursiveChildByName(Transform parent, string name, Boolean findInactive = false) {
            Transform result = null;

            foreach (Transform child in parent) {
                if (!findInactive && !child.gameObject.activeSelf)
                    continue;

                result = RecursiveChildByName(child, name, findInactive);

                if (null != result)
                    return result;
            }

            return (parent.name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ? parent : result;
        }

        public static GameObject[] RecursiveChildsByName(GameObject parent, string name, Boolean findInactive = false) {
            List<GameObject> result = new List<GameObject>();

            foreach (Transform child in parent.transform) {
                if (!findInactive && !child.gameObject.activeSelf)
                    continue;
                result.AddRange(RecursiveChildsByName(child.gameObject, name, findInactive));
            }

            if (parent.name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) {
                result.Add(parent);
            }
            return result.ToArray();
        }

        public static GameObject[] RecursiveChildsContainsName(GameObject parent, string name, Boolean findInactive = false) {
            List<GameObject> result = new List<GameObject>();

            foreach (Transform child in parent.transform) {
                if (!findInactive && !child.gameObject.activeSelf)
                    continue;
                result.AddRange(RecursiveChildsContainsName(child.gameObject, name, findInactive));
            }

            if (parent.name.ToLower().Contains(name)) {
                result.Add(parent);
            }
            return result.ToArray();
        }

        public static string FullNameByTransform(Transform child) {
            return (null == child.parent) ? child.name : FullNameByTransform(child.parent) + "." + child.name;
        }

        public static IEnumerable<Transform> SortChildByNames(this Transform transform) {
            List<Transform> rows = new List<Transform>();

            foreach (Transform child in transform) {
                rows.Add(child);
            }

            rows.Sort((a, b) => a.name.CompareTo(b.name));

            return rows;
        }

        private static bool CheckPositionInCameraView(Camera camera, Vector3 position) {
            position = camera.WorldToViewportPoint(position);
            return position.x >= 0.0f && position.x <= 1.0f && position.y >= 0.0f && position.y <= 1.0f;
        }

        public static bool IsObjectInCameraView(Camera camera, Renderer render) {
            return CheckPositionInCameraView(camera, render.bounds.min) && CheckPositionInCameraView(camera, render.bounds.max);
        }

        public static DotOrikoComponent GetDOComponent(this GameObject go) {
            return go.GetComponent<DotOrikoComponent>();
        }

        public static void ScaleInBack(this GameObject go, float time) {
            go.transform.localScale = Vector3.zero;
			go.transform.DOScale (Vector3.one, time).SetEase(Ease.InOutElastic);
        }

        public static void ScaleOutBack(this GameObject go, float time) {
			go.transform.DOScale (Vector3.zero, time).SetEase(Ease.InOutElastic);
        }

        public static void TweenParameter(this GameObject go, string callback, float time, float from, float to) {
            iTween.ValueTo(go, iTween.Hash(
                "from", from,
                "to", to,
                "time", time,
                "easetype", iTween.EaseType.easeInBack,
                "onupdate", callback
            ));
        }
    }
}