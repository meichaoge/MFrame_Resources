using UnityEngine;
using System.Collections;

namespace MFramework
{
    public class PerformanceManager : MonoBehaviour
    {
        private static PerformanceManager _instance;
        public static PerformanceManager GetInstance()
        {
            if (_instance == null)
            {
                GameObject buffer = new GameObject("PerformanceManager");
                _instance = buffer.AddComponent<PerformanceManager>();
                DontDestroyOnLoad(buffer);
            }
            return _instance;
        }

        /// <summary>
        /// 性能类型
        /// </summary>
        public enum PerformanceType
        {
            Low=1,
            Mid,
            High
        }

        private PerformanceType _performanceType;
        /// <summary>
        /// 性能模式
        /// </summary>
        public PerformanceType SystemPerformanceType
        {
            get
            {
                return _performanceType;
            }
            set
            {
                _performanceType = value;
                QualitySettings.SetQualityLevel((int)_performanceType, true);
                //switch (_performanceType)
                //{
                //    case PerformanceType.High:
                //        QualitySettings.SetQualityLevel((int)_performanceType, true);
                //  //      QualitySettings.currentLevel = QualityLevel.Fantastic;
                //        break;
                //    case PerformanceType.Low:
                //        QualitySettings.currentLevel = QualityLevel.Fastest;
                //        break;
                //    case PerformanceType.Mid:
                //        QualitySettings.currentLevel = QualityLevel.Good;
                //        break;
                //    default:
                //        Debug.LogError("SystemPerformanceType set Error!");
                //        break;
                //}
                //Debug.Log(QualitySettings.currentLevel);
                Debug.Log((PerformanceType)QualitySettings.GetQualityLevel());
            }
        }

        protected int mDropFrame = 30;
        protected float mJudgeTime = 30;
        protected float mFirstJudgeTime = 2;

        [SerializeField]
        /// <summary>
        /// 是否自动设置
        /// </summary>
        protected bool mIsAutomate=true;
        /// <summary>
        /// 是否首次
        /// </summary>
        protected bool mIsFirst;
        protected int mFrame;
        protected float mWaitTime;

        void Awake()
        {
            SystemPerformanceType = PerformanceType.High;
            mIsFirst = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (mIsAutomate)
            {
                mWaitTime += Time.deltaTime;
                mFrame++;

                if (mIsFirst)
                {
                    if (mWaitTime > mFirstJudgeTime)
                    {
                        if (mFrame / mWaitTime < mDropFrame)
                        {
                            DropLevel();
                        }

                        mFrame = 0;
                        mWaitTime = 0;
                    }
                    mIsFirst = false;
                    return;
                }

                if (mWaitTime > mJudgeTime)
                {
                    if (mFrame / mWaitTime < mDropFrame)
                    {
                        DropLevel();
                    }

                    mFrame = 0;
                    mWaitTime = 0;
                }
            }
        }

        private void DropLevel()
        {
            switch (SystemPerformanceType)
            {
                case PerformanceType.High:
                    SystemPerformanceType = PerformanceType.Mid;
                    break;
                case PerformanceType.Low:
                    SystemPerformanceType = PerformanceType.Low;
                    break;
                case PerformanceType.Mid:
                    SystemPerformanceType = PerformanceType.Low;
                    break;
                default:
                    Debug.LogError("SystemPerformanceType set Error!");
                    break;
            }
        }
    }
}