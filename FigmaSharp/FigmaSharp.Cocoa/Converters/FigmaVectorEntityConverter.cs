﻿/* 
 * FigmaVectorEntityConverter.cs
 * 
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
using System.Text;
using AppKit;

using FigmaSharp.Converters;
using FigmaSharp.Models;
using FigmaSharp.Services;
using FigmaSharp.Views;
using FigmaSharp.Views.Cocoa;

namespace FigmaSharp.Cocoa.Converters
{
    public class FigmaVectorEntityConverter : FigmaVectorEntityConverterBase
    {
        public override Type GetControlType(FigmaNode currentNode)
            => typeof(NSImageView);

        public override IView ConvertTo(FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
        {
            var vectorEntity = (FigmaVector)currentNode;
            var vector = new ImageView();
            var currengroupView = (NSImageView)vector.NativeObject;
            currengroupView.Configure(currentNode);

            if (vectorEntity.HasFills) {
                foreach (var fill in vectorEntity.fills) {
                    if (fill.type == "IMAGE") {
                        //we need to add this to our service
                    } else if (fill.type == "SOLID") {
                        if (fill.visible) {
                            //currengroupView.Layer.BackgroundColor = fill.color.ToCGColor ();
                        }
                    } else {
                        Console.WriteLine ($"NOT IMPLEMENTED FILL : {fill.type}");
                    }
                    //currengroupView.Layer.Hidden = !fill.visible;
                }
            }

            return vector;
        }

        public override string ConvertToCode(FigmaCodeNode currentNode, FigmaCodeNode parentNode, FigmaCodeRendererService rendererService)
        {
            var builder = new StringBuilder();
            if (rendererService.NeedsRenderConstructor (currentNode, parentNode))
                builder.WriteConstructor (currentNode.Name, GetControlType (currentNode.Node), rendererService.NodeRendersVar(currentNode, parentNode));
            builder.Configure((FigmaVector)currentNode.Node, Resources.Ids.Conversion.NameIdentifier);
            return builder.ToString();
        }
    }
}
