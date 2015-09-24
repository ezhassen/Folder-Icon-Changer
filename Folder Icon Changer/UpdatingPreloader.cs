using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UpdatingPreloaderEzz
{
    public partial class UpdatingPreloader : Form
    {
        public UpdatingPreloader()
        {
            sContext = SynchronizationContext.Current;
            InitializeComponent();
        }

        //bool updateText;
        private void UpdatingPreloader_TextChanged(object sender, EventArgs e)
        {
            if (this.Text != labelInfo.Text)
            {
                labelInfo.Text = this.Text;
            }
        }

        private void labelInfo_TextChanged(object sender, EventArgs e)
        {
            if (this.Text != labelInfo.Text)
            {
                this.Text = labelInfo.Text;
            }
        }

        [ThreadStatic]
        public SynchronizationContext sContext;
        private void UpdatingPreloader_Load(object sender, EventArgs e)
        {
            sContext = SynchronizationContext.Current;
        }

        public bool isSameSynchronizationContext(SynchronizationContext OtherSynchronizationContext)
        {
            return OtherSynchronizationContext == this.sContext;
        }

        public override string Text
        {
            get
            {
                if (DesignMode) return base.Text;
                if (isSameSynchronizationContext(SynchronizationContext.Current))
                {
                    return base.Text;
                }
                else
                {
                    var Res = "";
                    sContext.Send((s) => { Res = base.Text; }, null);
                    return Res;
                }
            }
            set
            {
                if (isSameSynchronizationContext(SynchronizationContext.Current))
                {
                    base.Text = value;
                    this.Update();
                }
                else
                {
                    sContext.Send((s) => { base.Text = value; this.Update(); }, null);
                }
            }
        }
    }
}
