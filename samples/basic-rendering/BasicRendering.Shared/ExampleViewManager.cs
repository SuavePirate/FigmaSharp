﻿/* 
 * Author:
 *   Jose Medrano <josmed@microsoft.com>
 *
 * Copyright (C) 2018 Microsoft, Corp
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit
 * persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
 * NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;

using FigmaSharp;
using FigmaSharp.Services;

namespace ExampleFigma
{
    public class ExampleViewManager 
    {
        const string fileName = "YdrY6p8JHY2UaKlSFgOwwUnd";
        readonly IScrollViewWrapper scrollViewWrapper;
        readonly IViewWrapper viewWrapper;
        readonly FigmaViewRendererService rendererService;
        readonly FigmaViewRendererDistributionService distributionService;
        readonly FigmaRemoteFileProvider fileProvider;

        public ExampleViewManager(IScrollViewWrapper scrollViewWrapper, IViewWrapper viewWrapper)
        {
            this.scrollViewWrapper = scrollViewWrapper;
            this.viewWrapper = viewWrapper;

            scrollViewWrapper.ContentView = viewWrapper;
         
            //we get the default specific view converters from each toolkit
            var converters = FigmaSharp.AppContext.Current.GetFigmaConverters();

            //TIP: the render consist in 2 steps:
            //1) generate all the views, decorating and calculate sizes
            //2) with this views we generate the hierarchy and position all the views based in the
            //native toolkit positioning system

            //in this case we want use a remote file provider (figma url from our document)
            fileProvider = new FigmaRemoteFileProvider();

            //we initialize our renderer service, this uses all the converters passed
            //and generate a collection of NodesProcessed which is basically contains <FigmaModel, IViewWrapper, FigmaParentModel>
            rendererService = new FigmaViewRendererService(fileProvider, converters);
            rendererService.Start(fileName, this.viewWrapper);

            //now we have all the views processed and the relationship we can distribute all the views into the desired base view
            distributionService = new FigmaViewRendererDistributionService(rendererService);
            distributionService.Start();

            //We want know the background color of the figma camvas and apply to our scrollview
            var canvas = fileProvider.Nodes.OfType<FigmaCanvas>().FirstOrDefault();
            if (canvas != null)
                scrollViewWrapper.BackgroundColor = canvas.backgroundColor;

            //NOTE: some toolkits requires set the real size of the content of the scrollview before position layers
            scrollViewWrapper.AdjustToContent();
        }
    }
}
