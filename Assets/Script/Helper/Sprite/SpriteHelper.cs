using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MFramework
{

    /// <summary>
    /// 获取打包图集中单张图片的简单方法   后面进行其他实现  临时实现
    /// </summary>
    public class Sprite_Texture_Helper : Singleton_Mono<Sprite_Texture_Helper>
    {
        [SerializeField]
        private Sprite[] m_SpriteArray;
        /// <summary>
        /// 实际存储的字典 关键字 SpriteName
        /// </summary>
        private Dictionary<string, Sprite> m_AllSprite = new Dictionary<string, Sprite>();

        [SerializeField]
        private Texture[] m_TextureArray;
        private Dictionary<string, Texture> m_AllTextures = new Dictionary<string, Texture>();

        void Start()
        {
            for (int _index = 0; _index < m_SpriteArray.Length; ++_index)
                m_AllSprite.Add(m_SpriteArray[_index].name, m_SpriteArray[_index]);

            for (int _index = 0; _index < m_TextureArray.Length; ++_index)
                m_AllTextures.Add(m_TextureArray[_index].name, m_TextureArray[_index]);
        }

        public Sprite GetSpriteByName(string _spriteName)
        {
            Sprite _newSprite;
            if (m_AllSprite.TryGetValue(_spriteName, out _newSprite))
                return _newSprite;
            return null;
        }


        public Texture GetTextureByName(string _textureName)
        {
            Texture _newTex;
            if (m_AllTextures.TryGetValue(_textureName, out _newTex))
                return _newTex;
            return null;
        }


    }
}
