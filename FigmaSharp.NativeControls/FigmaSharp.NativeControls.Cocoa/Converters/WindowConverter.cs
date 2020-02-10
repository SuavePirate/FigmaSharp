﻿/* 
 * CustomTextFieldConverter.cs
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
using AppKit;
using FigmaSharp.NativeControls.Base;
using System.Linq;
using System.Text;
using FigmaSharp.Cocoa;
using FigmaSharp.Models;
using FigmaSharp.Views;
using FigmaSharp.Services;

namespace FigmaSharp.NativeControls.Cocoa
{
	public class ImageRenderConverter : FigmaNativeControlConverter
	{
		public override bool CanConvert (FigmaNode currentNode)
		{
            return currentNode.name.StartsWith ("!image");
        }

		public override bool ScanChildren (FigmaNode currentNode)
		{
            return false;
		}

		public override IView ConvertTo (FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
		{
            var vector = new FigmaSharp.Views.Cocoa.ImageView ();
            var currengroupView = (NSImageView)vector.NativeObject;
            currengroupView.Configure (currentNode);
            return vector;
        }

		public override string ConvertToCode (FigmaCodeNode currentNode, FigmaCodeNode parentNode, FigmaCodeRendererService rendererService)
		{
            var builder = new StringBuilder ();
            if (rendererService.NeedsRenderConstructor (currentNode, parentNode))
                builder.WriteConstructor (currentNode.Name, typeof (NSImageView));
            builder.Configure (currentNode.Node, Resources.Ids.Conversion.NameIdentifier);
            return builder.ToString ();
        }
	}

	public class WindowStandardConverter : WindowStandardConverterBase
    {
        public override IView ConvertTo(FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
        {
            return null;
        }

        public override string ConvertToCode(FigmaCodeNode currentNode, FigmaCodeNode parentNode, FigmaCodeRendererService rendererService)
        {
            return string.Empty;
        }
    }

    public class WindowSheetConverter : WindowSheetConverterBase
    {
        public override IView ConvertTo (FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
        {
            return null;
        }

        public override string ConvertToCode (FigmaCodeNode currentNode, FigmaCodeNode parentNode, FigmaCodeRendererService rendererService)
        {
            return string.Empty;
        }
    }

    public class WindowPanelConverter : WindowPanelConverterBase
    {
        public override IView ConvertTo (FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
        {
            return null;
        }

        public override string ConvertToCode (FigmaCodeNode currentNode, FigmaCodeNode parentNode, FigmaCodeRendererService rendererService)
        {
            return string.Empty;
        }
    }
}
