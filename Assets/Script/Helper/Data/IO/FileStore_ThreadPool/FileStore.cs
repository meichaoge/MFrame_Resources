using System.Threading;


namespace MFramework
{

    ///文件保存子线程类
    /// </summary>
    public class FileStore
    {
        public enum DataType
        {
            ByteArray,
            String
        }

        string m_FilePath;
        object m_data;
        DataType type = DataType.ByteArray;

        void Store(object parameter)
        {
            if (m_data != null && string.IsNullOrEmpty(m_FilePath) == false)
            {
                //  byte[] data = m_Texture.EncodeToJPG();//**不能在线子程  但是可以放在协程
                switch (type)
                {
                    case DataType.ByteArray:
                        System.IO.File.WriteAllBytes(m_FilePath, (byte[])m_data);
                        break;
                    case DataType.String:
                        System.IO.File.WriteAllText(m_FilePath, (string)m_data);
                        break;
                }
            }
        }

        //Store File With Pool
        public static void FileStoreWithThreadPool(string path, object data, DataType _storeType = DataType.ByteArray)
        {
            FileStore loadThread = new FileStore();
            loadThread.m_FilePath = path;
            loadThread.m_data = data;
            loadThread.type = _storeType;
            ThreadPool.QueueUserWorkItem(new WaitCallback(loadThread.Store), new object()); // 参数可选
        }



    }


}
