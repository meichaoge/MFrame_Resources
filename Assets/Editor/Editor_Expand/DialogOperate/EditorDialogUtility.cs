using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// �༭��ģʽ�´򿪸��ֶԻ��������
    /// </summary>
    public class EditorDialogUtility 
    {

        /// <summary>
        /// ���ļ��Ի����ȡѡ���·��
        /// </summary>
        /// <param name="title">�Ի������</param>
        /// <param name="directory">��Ŀ¼</param>
        /// <param name="extension">��׺��</param>
        /// <returns></returns>
        public static string  OpenFileDialog(string title, string directory, string extension)
        {
            return EditorUtility.OpenFilePanel(title, directory, extension);
        }

        /// <summary>
        /// �򿪱����ļ��еĶԻ���
        /// </summary>
        /// <param name="title">�Ի������</param>
        /// <param name="folder">������ļ���</param>
        /// <param name="defaultName">Ĭ����</param>
        /// <returns></returns>
        public static string SaveFolderDialog(string title, string folder, string defaultName)
        {
            return EditorUtility.SaveFolderPanel(title, folder, defaultName);
        }

        /// <summary>
        /// �򿪱����ļ��ĶԻ���
        /// </summary>
        /// <param name="title">�Ի������</param>
        /// <param name="directory">�����Ŀ¼</param>
        /// <param name="defaultName">Ĭ����</param>
        /// <param name="extension">��׺��(����Ҫ . )</param>
        /// <returns></returns>
        public static string SaveFileDialog(string title, string directory, string defaultName, string extension)
        {
            return EditorUtility.SaveFilePanel(title, directory, defaultName, extension);
        }

    }
}
