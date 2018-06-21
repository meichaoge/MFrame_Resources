using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.EditorExpand
{
    public class SetTextureFormat_Editor : Editor
    {
        /// <summary>
        /// 导入图片后执行
        /// </summary>
        /// <param name="texture"></param>
        void OnPostprocessTexture(Texture2D texture)
        {
            //string assetPath = AssetDatabase.GetAssetPath(texture);
            //Debug.Log("OnPostprocessTexture  assetPath"+ assetPath);
            //TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            //if (textureImporter != null)
            //{
            //    Debug.Log("OnPostprocessTexture");
            //    string AtlasName = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(assetPath)).Name;
            //    textureImporter.textureType = TextureImporterType.Sprite;
            //    textureImporter.spriteImportMode = SpriteImportMode.Single;
            //    textureImporter.spritePackingTag = AtlasName;
            //    textureImporter.mipmapEnabled = false;
            //}
        }

        /// <summary>
        /// 导入图片前执行
        /// </summary>
        void OnPreprocessTexture()
        {
            //Debug.Log("OnPreprocessTexture");

            //string assetPath = AssetDatabase.GetAssetPath(texture);
            //Debug.Log("OnPostprocessTexture  assetPath" + assetPath);
            //TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            //if (textureImporter != null)
            //{
            //    Debug.Log("OnPostprocessTexture");
            //    string AtlasName = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(assetPath)).Name;
            //    textureImporter.textureType = TextureImporterType.Sprite;
            //    textureImporter.spriteImportMode = SpriteImportMode.Single;
            //    textureImporter.spritePackingTag = AtlasName;
            //    textureImporter.mipmapEnabled = false;
            //}
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
        {
            foreach (string move in movedAssets)
            {
                //这里重新 import一下
                AssetDatabase.ImportAsset(move);
            }
        }

    }

}