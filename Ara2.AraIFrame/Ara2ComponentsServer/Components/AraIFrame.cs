// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Ara2;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vConteiner:false,vResizable:false)]
    public class AraIFrame : AraComponentVisualAnchor, IAraDev
    {
        
        public AraIFrame(IAraContainerClient ConteinerFather)
            : this(ConteinerFather, "?araignore=1")
        {
            Width = 200;
            Height = 200;
        }

        public AraIFrame(IAraContainerClient ConteinerFather,string vSrc)
            : base(AraObjectClienteServer.Create(ConteinerFather, "iframe", new Dictionary<string, string> { { "src", vSrc } }), ConteinerFather, "AraIFrame")
        {
            _Src = vSrc;
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            Menssage = new AraComponentEvent<DMenssage>(this, "Menssage");

            this.EventInternal += AraButton_EventInternal;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraIFrame/AraIFrame.js");
        }

        public void AraButton_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    if (Enabled)
                        Click.InvokeEvent(this, new EventArgs());
                break;
                case "MENSSAGE":
                {
                    if (Menssage.InvokeEvent != null)
                    {
                        Tick vTick = Tick.GetTick();
                        try
                        {
                            Menssage.InvokeEvent(vTick.Page.Request["vMenssage"].ToString());
                        }
                        catch (Exception err)
                        {
                            throw new Exception("Erro on event 'Menssage' data '" + vTick.Page.Request["vMenssage"].ToString() + "'.\n" + err.Message);
                        }
                    }
                }
                break;
            }
        }

        #region Eventos
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click;

        [AraDevEvent]
        public AraComponentEvent<DMenssage> Menssage;
        #endregion

        private bool _Visible = true;
        [AraDevProperty(true)]
        [PropertySupportLayout]
        public bool Visible
        {
            set
            {
                _Visible = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetVisible(" + (_Visible == true ? "true" : "false") + "); \n");
            }
            get { return _Visible; }
        }

        private bool _Enabled = true;
        [AraDevProperty(true)]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetEnabled(" + (_Enabled == true ? "true" : "false") + "); \n");
            }
        }

        private string _Src;
        [AraDevProperty("")]
        public string Src
        {
            get { return _Src; }
            set
            {
                _Src = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetSrc('" + _Src.Replace("'", "\\'") + "'); \n");
            }
        }

        private bool _BorderVisible = true;
        [AraDevProperty(true)]
        public bool BorderVisible
        {
            get { return _BorderVisible; }
            set
            {
                _BorderVisible = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetBorderVisible(" + (_BorderVisible ? "true" : "false") + "); \n");
            }
        }

        private bool _EnableSupportForGetClient = false;
        [AraDevProperty(false)]
        public bool EnableSupportForGetClient
        {
            get { return _EnableSupportForGetClient; }
            set
            {
                _EnableSupportForGetClient = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetEnableSupportForGetClient(" + (_EnableSupportForGetClient ? "true" : "false") + "); \n");
            }
        }

        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }

        #endregion
    }
}
