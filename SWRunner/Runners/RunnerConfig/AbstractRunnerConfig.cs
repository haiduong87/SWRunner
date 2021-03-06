﻿using System;
using System.Xml.Serialization;
using System.Windows;
using System.Drawing;

namespace SWRunner.Runners
{
    [XmlInclude(typeof(CairosRunnerConfig))]
    [Serializable, XmlRoot(ElementName = "RunConfig")]
    public abstract class AbstractRunnerConfig
    {
        // Basic
        public PointF StartPoint;
        public PointF ReplayPoint;
        public PointF NoRevivePoint;

        // Refill
        public PointF OpenShopPoint;
        public PointF BuyEnergyWithCrystalPoint;
        public PointF ConfirmBuyPoint;
        public PointF BuyOKPoint;
        public PointF CloseShopPoint;

        // Event
        public PointF EventOKPoint;
    }

}
