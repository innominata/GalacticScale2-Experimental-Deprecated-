using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GalacticScale
{
    public static class SystemDisplay
    {
        public static bool inSystemDisplay = false;
        private static StarData viewStar;
        public static Button backButton;
        public static int customBirthStar = -1;
        public static int customBirthPlanet = -1;
        public static bool pressSpamProtector = false;

        public static void AbortRender(UIVirtualStarmap starmap)
        {
            // Modeler.Reset();
            ShowStarMap(starmap);
        }
        public static void OnUpdate(UIVirtualStarmap starmap, int starIndex)
        {
           
        }
        
        public static void OnBackClick(UIGalaxySelect instance)
        {
            GS2.Warn("BackClick");
            if (!inSystemDisplay) instance.CancelSelect();
            else ShowStarMap(instance.starmap);
        }

        public static void OnStarMapClick(UIVirtualStarmap starmap, int starIndex)
        {
            GS2.Warn($"StarmapClick { starIndex}");
            switch (starIndex)
            {
                case -1 when (UIRoot.instance.uiGame.planetDetail.gameObject.activeSelf):
                    GS2.Warn("Hiding Planet Details");
                    HidePlanetDetail();
                    break;
                case -1 when (!UIRoot.instance.uiGame.planetDetail.gameObject.activeSelf):
                    GS2.Warn("Hiding SolarSystem");
                    ShowStarMap(starmap);
                    break;
                case 0 when inSystemDisplay:
                    GS2.Warn("System Star Click");
                    OnSolarSystemStarClick(starmap);
                    break;
                case int x when x >= 0 && inSystemDisplay:
                    GS2.Warn($"PlanetClick {starIndex}");
                    OnSolarSystemPlanetClick(starmap, starIndex);
                    break;
                case int x when x >= 0 && !inSystemDisplay:                   ;
                    GS2.Warn($"- {starIndex} OnStarMapClick");
                    OnStarClick(starmap, starIndex);
                    break;
                default:
                    GS2.Warn($"Clicked Starmap with Erroneous starIndex of {starIndex}");
                    break;
            }
        }
        public static void OnStarMapRightClick(UIVirtualStarmap starmap, int starIndex)
        {
            GS2.Warn($"StarmapRightClick { starIndex}");
            switch (starIndex)
            {
                case -1 when (UIRoot.instance.uiGame.planetDetail.gameObject.activeSelf):
                    GS2.Warn("Rightclick Hiding Details");
                    HidePlanetDetail();
                    ShowStarCount();
                    break;
                case -1 when (!UIRoot.instance.uiGame.planetDetail.gameObject.activeSelf):
                    GS2.Warn("RightClick Hiding SolarSystem");
                    ShowStarMap(starmap);
                    break;
                case 0 when inSystemDisplay:
                    GS2.Warn("System Star RightClick");
                    OnSolarSystemStarClick(starmap);
                    break;
                case int x when x >= 0 && inSystemDisplay:
                    GS2.Warn($"Planet RightClick {starIndex}");
                    OnSolarSystemPlanetClick(starmap, starIndex);
                    break;
                case int x when x >= 0 && !inSystemDisplay:
                    ;
                    GS2.Warn($"- {starIndex} OnStarMapRightClick");
                    OnStarClick(starmap, starIndex);
                    break;
                default:
                    GS2.Warn($"RightClicked Starmap with Erroneous starIndex of {starIndex}");
                    break;
            }
        }
        public static void OnSolarSystemStarClick(UIVirtualStarmap starmap)
        {
            HidePlanetDetail();
            HideStarCount();
            ShowStarDetail(viewStar);
        }
        public static void HideStarCount()
        {
            GameObject.Find("UI Root/Overlay Canvas/Galaxy Select/right-group")?.SetActive(false);
        }
        public static void HidePlanetDetail()
        {
            UIRoot.instance.uiGame.planetDetail.gameObject.SetActive(false);
        }
        public static void HideStarDetail()
        {
            UIRoot.instance.uiGame.starDetail.gameObject.SetActive(false);
        }
        public static void ShowPlanetDetail(PlanetData pData)
        {
            UIRoot.instance.uiGame.SetPlanetDetail(pData);
            UIRoot.instance.uiGame.planetDetail.gameObject.SetActive(true);
            UIRoot.instance.uiGame.planetDetail.gameObject.GetComponent<RectTransform>().parent.gameObject.SetActive(true);
            UIRoot.instance.uiGame.planetDetail.gameObject.GetComponent<RectTransform>().parent.gameObject.GetComponent<RectTransform>().parent.gameObject.SetActive(true);
            UIRoot.instance.uiGame.planetDetail._OnUpdate();
        }
        public static void ShowStarDetail(StarData starData)
        {
            UIRoot.instance.uiGame.SetStarDetail(starData);
            UIRoot.instance.uiGame.starDetail.gameObject.SetActive(true);
            UIRoot.instance.uiGame.starDetail.gameObject.GetComponent<RectTransform>().parent.gameObject.SetActive(true);
            UIRoot.instance.uiGame.starDetail.gameObject.GetComponent<RectTransform>().parent.gameObject.GetComponent<RectTransform>().parent.gameObject.SetActive(true);
            UIRoot.instance.uiGame.starDetail._OnUpdate();
        }
        public static void OnStarClick(UIVirtualStarmap starmap, int starIndex)
        {
            viewStar = starmap.starPool[starIndex].starData;
            ClearStarmap(starmap);
            GS2.Warn($"OnStarClick {viewStar.name}");
            ShowSolarSystem(starmap, starIndex);
        }

        public static void OnSolarSystemPlanetClick(UIVirtualStarmap starmap, int clickIndex)
        {
            int planetIndex = clickIndex -1;
            GS2.Warn($"System:{viewStar.name} PlanetCount:{viewStar.planetCount}");
            PlanetData pData = viewStar?.planets[planetIndex];
            GS2.Warn("C");
            if (pData == null) return;
            GS2.Warn($"{pData.name}");
            HideStarDetail();
            HideStarCount();
            ShowPlanetDetail(pData);
        }
        public static void ShowStarCount()
        {
            GameObject.Find("UI Root/Overlay Canvas/Galaxy Select/right-group").SetActive(true);

            
        }
        public static void ShowStarMap(UIVirtualStarmap starmap)
        {
            GS2.Warn("Reverting to galaxy view");
            inSystemDisplay = false;
            viewStar = null;
            starmap.clickText = "";
            HidePlanetDetail();
            HideStarDetail();
            ClearStarmap(starmap);
            ShowStarCount();
            starmap.OnGalaxyDataReset();
        }
        public static void ShowSolarSystem(UIVirtualStarmap starmap, int starIndex)
        {
            ClearStarmap(starmap);
            inSystemDisplay = true;
            GS2.Warn("ShowSolarSystem");
            // start planet compute thread if not done already
            // Modeler.aborted = false;
            PlanetModelingManager.StartPlanetComputeThread();

            // add star
            StarData starData = starmap._galaxyData.StarById(starIndex + 1); // because StarById() decrements by 1
            AddStarToStarmap(starmap, starData);
            
            var starScale = starmap.starPool[0].starData.radius /40f * GS2.Config.VirtualStarmapStarScaleFactor;
            GS2.Warn($"Scale : {starScale} Radius:{starmap.starPool[0].starData.radius} OrigScale:{starmap.starPool[0].pointRenderer.transform.localScale}");
            starmap.starPool[0].pointRenderer.transform.localScale = new Vector3(starScale,starScale,starScale);
            //starmap.clickText = starData.id.ToString();
            //Debug.Log("Setting it to " + starmap.clickText + " " + starData.id);

            for (int i = 0; i < starData.planetCount; i++)
            {
                // add planets
                PlanetData pData = starData.planets[i];
                Color color = starmap.neutronStarColor;
                
                bool isMoon = false;

                VectorLF3 pPos = GetRelativeRotatedPlanetPos(starData, pData, ref isMoon);
                // GS2.Warn("ShowSolarSystem2");
                // request generation of planet surface data to display its details when clicked and if not already loaded
                //if (!pData.loaded) PlanetModelingManager.RequestLoadPlanet(pData);
                // GS2.Warn("ShowSolarSystem3");
                // create fake StarData to pass _OnLateUpdate()
                StarData dummyStarData = new StarData();
                dummyStarData.position = pPos;
                var gsPlanet = GS2.GetGSPlanet(pData);
                var gsTheme = gsPlanet.GsTheme;
                var orbitColor = PlanetTemperatureToStarColor(gsTheme.Temperature);
                var planetColor = (gsTheme.minimapMaterial.Colors.ContainsKey("_Color"))? gsTheme.minimapMaterial.Colors["_Color"]:Color.magenta;
                GS2.Log($"Color of {starData.name} a {starData.typeString} star is {starData.color}");
                dummyStarData.id = pData.id;

                Vector3 scale = Vector3.one * GS2.Config.VirtualStarmapScaleFactor * (pData.realRadius / 1000);
                // if (scale.x > 3 || scale.y > 3 || scale.z > 3)
                // {
                //     scale = new Vector3(3, 3, 3);
                // }

                // GS2.Warn($"ShowSolarSystem4 {i}");
                // GS2.Warn($"Planet: {starData.planets[i].name}");
                // GS2.Warn($"Pool Length: {i + 1} / {starmap.starPool.Count}");
                while (starmap.starPool.Count <= i + 1)
                {
                    UIVirtualStarmap.StarNode starNode2 = new UIVirtualStarmap.StarNode();
                    starNode2.active = false;
                    starNode2.starData = null;
                    starNode2.pointRenderer = Object.Instantiate<MeshRenderer>(starmap.starPointPrefab, starmap.starPointPrefab.transform.parent);
                    starNode2.nameText = Object.Instantiate<Text>(starmap.nameTextPrefab, starmap.nameTextPrefab.transform.parent);
                    starmap.starPool.Add(starNode2);
                }

                starmap.starPool[i + 1].active = true;

                // GS2.Warn("ShowSolarSystem4a");

                starmap.starPool[i + 1].starData = dummyStarData;
                starmap.starPool[i + 1].pointRenderer.material.SetColor("_TintColor", planetColor);
                starmap.starPool[i + 1].pointRenderer.transform.localPosition = pPos;
                starmap.starPool[i + 1].pointRenderer.transform.localScale = scale;
                starmap.starPool[i + 1].pointRenderer.gameObject.SetActive(true);
                starmap.starPool[i + 1].nameText.text = pData.displayName + " (" + pData.typeString + ")";
                starmap.starPool[i + 1].nameText.color = Color.Lerp(planetColor, Color.white, 0.5f);
                starmap.starPool[i + 1].nameText.rectTransform.sizeDelta = new Vector2(starmap.starPool[i + 1].nameText.preferredWidth, starmap.starPool[i + 1].nameText.preferredHeight);
                starmap.starPool[i + 1].nameText.rectTransform.anchoredPosition = new Vector2(-2000f, -2000f);
                starmap.starPool[i + 1].textContent = pData.displayName + " (" + pData.typeString + ")";

                starmap.starPool[i + 1].nameText.gameObject.SetActive(true);
                // GS2.Warn($"ShowSolarSystem5 {i} / {starmap.connPool.Count}");
                // add orbit renderer
                while (starmap.connPool.Count <= i)
                {
                    starmap.connPool.Add(new UIVirtualStarmap.ConnNode
                    {
                        active = false,
                        starA = null,
                        starB = null,
                        lineRenderer = Object.Instantiate(starmap.connLinePrefab, starmap.connLinePrefab.transform.parent)
                    });
                    //starmap.connPool[starmap.connPool.Count-1].lineRenderer.material.SetColor("_LineColorA", Color.Lerp(color, Color.white, 0.65f));
                    //starmap.connPool[starmap.connPool.Count - 1].lineRenderer.material.SetColor("_LineColorB", Color.Lerp(color, Color.white, 0.65f));
                }

                //if (starmap.connPool.Count -1 >= i) {
                starmap.connPool[i].active = true;
                starmap.connPool[i].lineRenderer.material.SetColor("_LineColorA", Color.Lerp(starmap.starColors.Evaluate(orbitColor), Color.white, 0.65f));
                starmap.connPool[i].lineRenderer.material.SetColor("_LineColorB", Color.Lerp(starmap.starColors.Evaluate(orbitColor), Color.white, 0.25f));
                if (starmap.connPool[i].lineRenderer.positionCount != 61)
                {
                    starmap.connPool[i].lineRenderer.positionCount = 61;
                }

                //}
                // GS2.Warn("ShowSolarSystem6");
                for (int j = 0; j < 61; j++)
                {
                    // GS2.Warn("ShowSolarSystem7");
                    float f = (float)j * 0.017453292f * 6f; // ty dsp devs :D
                    Vector3 cPos = GetCenterOfOrbit(starData, pData, ref isMoon);
                    Vector3 position;
                    if (isMoon)
                    {
                        position = new Vector3(Mathf.Cos(f) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor * 8 + (float)cPos.x, cPos.y, Mathf.Sin(f) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor * 8 + (float)cPos.z);
                    }
                    else
                    {
                        position = new Vector3(Mathf.Cos(f) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor + (float)cPos.x, cPos.y, Mathf.Sin(f) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor + (float)cPos.z);
                    }

                    // GS2.Warn("ShowSolarSystem7a");

                    // rotate position around center by orbit angle
                    Quaternion quaternion = Quaternion.Euler(pData.orbitInclination, pData.orbitInclination, pData.orbitInclination);
                    Vector3 dir = quaternion * (position - cPos);
                    position = dir + cPos;

                    starmap.connPool[i].lineRenderer.SetPosition(j, position);
                }

                // GS2.Warn("ShowSolarSystem7b");

                starmap.connPool[i].lineRenderer.gameObject.SetActive(true);
            }
        }
        private static VectorLF3 GetCenterOfOrbit(StarData starData, PlanetData pData, ref bool isMoon)
        {
            // GS2.Warn("GetCenterOfOrbit");
            if (pData.orbitAroundPlanet != null)
            {
                return GetRelativeRotatedPlanetPos(starData, pData.orbitAroundPlanet, ref isMoon);
            }

            isMoon = false;
            return starData.position;
        }

        private static VectorLF3 GetRelativeRotatedPlanetPos(StarData starData, PlanetData pData, ref bool isMoon)
        {
            // GS2.Warn("GetRelativeRotatedPlanetPos");
            VectorLF3 pos;
            VectorLF3 dir;
            Quaternion quaternion;
            if (pData.orbitAroundPlanet != null)
            {
                VectorLF3 centerPos = GetRelativeRotatedPlanetPos(starData, pData.orbitAroundPlanet, ref isMoon);
                isMoon = true;
                pos = new VectorLF3(Mathf.Cos(pData.orbitPhase) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor * 8 + centerPos.x, centerPos.y, Mathf.Sin(pData.orbitPhase) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor * 8 + centerPos.z);
                quaternion = Quaternion.Euler(pData.orbitInclination, pData.orbitInclination, pData.orbitInclination);
                dir = quaternion * (pos - centerPos);
                return dir + centerPos;
            }

            pos = new VectorLF3(Mathf.Cos(pData.orbitPhase) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor + starData.position.x, starData.position.y, Mathf.Sin(pData.orbitPhase) * pData.orbitRadius * GS2.Config.VirtualStarmapOrbitScaleFactor + starData.position.z);
            quaternion = Quaternion.Euler(pData.orbitInclination, pData.orbitInclination, pData.orbitInclination);
            dir = quaternion * (pos - starData.position);
            return dir + starData.position;
        }

        // probably reverse patch this if there is time
        private static void AddStarToStarmap(UIVirtualStarmap starmap, StarData starData)
        {
            // GS2.Warn("AddStarToStarMap");
            Color color = starmap.starColors.Evaluate(starData.color);
            if (starData.type == EStarType.NeutronStar)
            {
                color = starmap.neutronStarColor;
            }
            else if (starData.type == EStarType.WhiteDwarf)
            {
                color = starmap.whiteDwarfColor;
            }
            else if (starData.type == EStarType.BlackHole)
            {
                color = starmap.blackholeColor;
            }

            float num2 = 1.2f;
            if (starData.type == EStarType.GiantStar)
            {
                num2 = 3f;
            }
            else if (starData.type == EStarType.WhiteDwarf)
            {
                num2 = 0.6f;
            }
            else if (starData.type == EStarType.NeutronStar)
            {
                num2 = 0.6f;
            }
            else if (starData.type == EStarType.BlackHole)
            {
                num2 = 0.8f;
            }

            string text = starData.displayName + "  ";
            if (starData.type == EStarType.GiantStar)
            {
                if (starData.spectr <= ESpectrType.K)
                {
                    text += "红巨星".Translate();
                }
                else if (starData.spectr <= ESpectrType.F)
                {
                    text += "黄巨星".Translate();
                }
                else if (starData.spectr == ESpectrType.A)
                {
                    text += "白巨星".Translate();
                }
                else
                {
                    text += "蓝巨星".Translate();
                }
            }
            else if (starData.type == EStarType.WhiteDwarf)
            {
                text += "白矮星".Translate();
            }
            else if (starData.type == EStarType.NeutronStar)
            {
                text += "中子星".Translate();
            }
            else if (starData.type == EStarType.BlackHole)
            {
                text += "黑洞".Translate();
            }
            else if (starData.type == EStarType.MainSeqStar)
            {
                text = text + starData.spectr.ToString() + "型恒星".Translate();
            }

            if (starData.index == ((customBirthStar != -1) ? customBirthStar - 1 : starmap._galaxyData.birthStarId - 1))
            {
                text = "即将登陆".Translate() + "\r\n" + text;
            }

            starmap.starPool[0].active = true;
            starmap.starPool[0].starData = starData;
            starmap.starPool[0].pointRenderer.material.SetColor("_TintColor", color);
            starmap.starPool[0].pointRenderer.transform.localPosition = starData.position;
            starmap.starPool[0].pointRenderer.transform.localScale = Vector3.one * num2 * 2;
            starmap.starPool[0].pointRenderer.gameObject.SetActive(true);
            starmap.starPool[0].nameText.text = text;
            starmap.starPool[0].nameText.color = Color.Lerp(color, Color.white, 0.5f);
            starmap.starPool[0].nameText.rectTransform.sizeDelta = new Vector2(starmap.starPool[0].nameText.preferredWidth, starmap.starPool[0].nameText.preferredHeight);
            starmap.starPool[0].nameText.rectTransform.anchoredPosition = new Vector2(-2000f, -2000f);
            starmap.starPool[0].textContent = text;

            starmap.starPool[0].nameText.gameObject.SetActive(true);
        }
        public static void ClearStarmap(UIVirtualStarmap starmap)
        {
            GS2.Warn("ClearStarmap");

            // GameObject.DestroyImmediate(starmap.starPointBirth.gameObject);
            
            foreach (UIVirtualStarmap.StarNode starNode in starmap.starPool)
            {
                starNode.active = false;
                starNode.starData = null;
                starNode.pointRenderer.gameObject.SetActive(false);
                starNode.nameText.gameObject.SetActive(false);
            }
            
            foreach (UIVirtualStarmap.ConnNode connNode in starmap.connPool)
            {
                connNode.active = false;
                connNode.starA = null;
                connNode.starB = null;
                connNode.lineRenderer.gameObject.SetActive(false);
            }
            // starmap = new UIVirtualStarmap();
            // starmap.starPool = new List<UIVirtualStarmap.StarNode>();
            // starmap.connPool = new List<UIVirtualStarmap.ConnNode>();
            
        }

        private static float PlanetTemperatureToStarColor(float temperature)
        {
            if (temperature > 3) return 0;
            if (temperature > 1) return 0.3f;
            if (temperature > -1) return 0.5f;
            if (temperature > -3.1f) return 0.8f;
            return 0.9f;
        }
    }
}