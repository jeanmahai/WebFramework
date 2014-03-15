using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using VSLangProj;
namespace AngularAddin
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect:IDTExtensibility2
    {
        public DocumentEvents DocEvt { get; private set; }
        public SolutionEvents SlnEvt { get; private set; }
        public Solution CurrentSln { get; private set; }
        public ProjectItemsEvents PjtEvt { get; private set; }
        private ProjectItem TempItem { get; set; }

        /// <summary>
        /// 向output中输出消息
        /// </summary>
        /// <param name="message"></param>
        private void Display(string message)
        {
            var outputWin = this._applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindOutput).Object as OutputWindow;
            if (outputWin == null) return;
            outputWin.ActivePane.Activate();
            outputWin.ActivePane.OutputString(message + "\n");
        }

        private void SaveFile(string path,string val)
        {
            var bytes = Encoding.Default.GetBytes(val);
            using (var fs = new FileStream(path,FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite))
            {
                fs.SetLength(0);
                fs.Write(bytes,0,bytes.Length);
            }
        }
        private void SaveFile(string path,byte[] val)
        {

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            using (var fs = new FileStream(path,FileMode.Open))
            {
                fs.Write(val,0,val.Length);
            }
        }

        private void SaveAngularView(Document doc)
        {
            var text = GetText(doc).Replace("\n","").Replace("\r","").Replace("\t","");

            var tmpl = new AngularViewTmpl();
            tmpl.Session = new Dictionary<string,object>();
            tmpl.Session.Add("Html",text);
            tmpl.Initialize();

            var val = tmpl.TransformText();

            var name = doc.Name.Substring(0,doc.Name.IndexOf(".")) + ".js";
            var dir = Path.GetDirectoryName(doc.FullName);
            var path = Path.Combine(dir,name);
            SaveFile(path,val);
        }

        private string GetText(Document doc)
        {
            using (var sr = new StreamReader(doc.FullName,Encoding.Default))
            {
                return sr.ReadToEnd();
            }
        }

        private bool IsAngularViewTmpl(string fileName)
        {
            fileName = fileName.ToUpper();
            if (fileName.EndsWith(".AGV.HTM")) return true;
            if (fileName.EndsWith(".AGV.HTML")) return true;
            return false;
        }

        private bool IsAngularView(string fileName)
        {
            fileName = fileName.ToUpper();
            if (fileName.EndsWith(".AGV.JS")) return true;
            return false;
        }

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {

        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application,ext_ConnectMode connectMode,object addInInst,ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            Display("angular addin connected.");

            SlnEvt = this._applicationObject.Events.SolutionEvents;
            SlnEvt.Opened += new _dispSolutionEvents_OpenedEventHandler(SlnEvt_Opened);
        }

        void PjtEvt_ItemAdded(ProjectItem ProjectItem)
        {
            Display("added a new item.");
            //判断是不是编辑angular view tmpl
            var curPjt = ((Array)CurrentSln.DTE.ActiveSolutionProjects).GetValue(0) as Project;
            if (IsAngularViewTmpl(ProjectItem.Name))
            {
                var name = ProjectItem.Name.Substring(0,ProjectItem.Name.IndexOf(".")) + ".agv.js";
                var dir = Path.GetDirectoryName(ProjectItem.FileNames[0]);
                var path = Path.Combine(dir,name);

                if (!File.Exists(path))
                {
                    File.Create(path);
                    //TODO 添加到当前工程中,并和当前文档建立文件组
                    TempItem = ProjectItem;
                    curPjt.ProjectItems.AddFromFile(path);
                }
            }
            if (IsAngularView(ProjectItem.Name))
            {
                if (TempItem != null)
                {
                    TempItem.ProjectItems.AddFromFile(ProjectItem.FileNames[0]);
                    TempItem = null;
                }
            }
        }

        void DocEvt_DocumentOpened(Document Document)
        {
            //判断是不是编辑angular view
            if (!IsAngularViewTmpl(Document.Name)) return;
            var name = Document.Name.Substring(0,Document.Name.IndexOf(".")) + ".js";
            var dir = Path.GetDirectoryName(Document.FullName);
            var path = Path.Combine(dir,name);

            if (!File.Exists(path))
            {
                File.Create(path);
                //TODO 添加到当前工程中,并和当前文档建立文件组
                var curPjt = ((Array)CurrentSln.DTE.ActiveSolutionProjects).GetValue(0) as Project;
                curPjt.ProjectItems.AddFromFile(path);
            }
        }

        void SlnEvt_Opened()
        {
            //获取当前的解决方案
            CurrentSln = this._applicationObject.DTE.Solution;
            Display("打开sln");
            PjtEvt = this._applicationObject.Events.GetObject("CSharpProjectItemsEvents") as ProjectItemsEvents;
            if (PjtEvt != null)
            {
                PjtEvt.ItemAdded += new _dispProjectItemsEvents_ItemAddedEventHandler(PjtEvt_ItemAdded);
            }

            DocEvt = this._applicationObject.Events.DocumentEvents;
            DocEvt.DocumentSaved += docEvt_DocumentSaved;
            //DocEvt.DocumentOpened += new _dispDocumentEvents_DocumentOpenedEventHandler(DocEvt_DocumentOpened);
        }


        void docEvt_DocumentSaved(Document Document)
        {
            if (!IsAngularViewTmpl(Document.Name)) return;

            SaveAngularView(Document);
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode,ref Array custom)
        {

        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}