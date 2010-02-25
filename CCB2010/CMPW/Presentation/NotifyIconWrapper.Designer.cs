namespace JOYFULL.CMPW.Presentation
{
    partial class NotifyIconWrapper
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose( bool disposing )
        {
            if( NotifyIco != null )
            {
                NotifyIco.Visible = false;
                NotifyIco.Dispose();
                NotifyIco = null;
            }
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.NotifyIco = new System.Windows.Forms.NotifyIcon(this.components);
            // 
            // NotifyIco
            // 
            this.NotifyIco.Text = "集中监控预警系统";
            this.NotifyIco.Visible = true;

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotifyIco;
    }
}
