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
using System;
using FigmaSharp.Cocoa;
using FigmaSharp.Models;
using System.Linq;
using FigmaSharp.Views;
using FigmaSharp.Services;
using FigmaSharp.Views.Cocoa;
using System.Text;

namespace FigmaSharp.NativeControls.Cocoa
{
	public class ComboBoxConverter : ComboBoxConverterBase
	{
		public override IView ConvertTo (FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
		{
			var view = new NSComboBox ();
			var figmaInstance = (FigmaFrameEntity)currentNode;
			view.Configure (currentNode);

			figmaInstance.TryGetNativeControlComponentType (out var controlType);
			switch (controlType) {
				case NativeControlComponentType.ComboBoxSmall:
				case NativeControlComponentType.ComboBoxSmallDark:
					view.ControlSize = NSControlSize.Small;
					break;
				case NativeControlComponentType.ComboBoxStandard:
				case NativeControlComponentType.ComboBoxStandardDark:
					view.ControlSize = NSControlSize.Regular;
					break;
			}

			var label = figmaInstance.children
			   .OfType<FigmaText> ()
			   .FirstOrDefault (s => s.name == "lbl");

			if (label != null && !string.IsNullOrEmpty (label.characters)) {
				view.Add (new Foundation.NSString (label.characters));
			}

			if (controlType.ToString ().EndsWith ("Dark", StringComparison.Ordinal)) {
				view.Appearance = NSAppearance.GetAppearance (NSAppearance.NameDarkAqua);
			} else {
				view.Appearance = NSAppearance.GetAppearance (NSAppearance.NameVibrantLight);
			}

			return new View (view);
		}

		public override string ConvertToCode (FigmaCodeNode currentNode, FigmaCodeNode parentNode, FigmaCodeRendererService rendererService)
		{
			var figmaInstance = (FigmaFrameEntity)currentNode.Node;

			var builder = new StringBuilder ();
			var name = FigmaSharp.Resources.Ids.Conversion.NameIdentifier;

			if (rendererService.NeedsRenderConstructor (currentNode, parentNode))
				builder.WriteConstructor (name, typeof (NSComboBox).FullName);

			builder.Configure (currentNode.Node, name);

			figmaInstance.TryGetNativeControlComponentType (out var controlType);
			switch (controlType) {
				case NativeControlComponentType.PopUpButtonSmall:
				case NativeControlComponentType.PopUpButtonSmallDark:
					builder.WriteEquality (name, nameof (NSButton.ControlSize), NSControlSize.Small);
					break;
				case NativeControlComponentType.PopUpButtonStandard:
				case NativeControlComponentType.PopUpButtonStandardDark:
					builder.WriteEquality (name, nameof (NSButton.ControlSize), NSControlSize.Regular);
					break;
			}

			var label = figmaInstance.children
		   .OfType<FigmaText> ()
		   .FirstOrDefault (s => s.name == "lbl");

			if (label != null && !string.IsNullOrEmpty (label.characters)) {
				var nsstringcontructor = typeof (Foundation.NSString).GetConstructor (new[] { $"\"{label.characters}\"" });
				builder.WriteMethod (name, nameof (NSComboBox.Add), nsstringcontructor);
			}
			//if (controlType.ToString ().EndsWith ("Dark", StringComparison.Ordinal)) {
			//	builder.AppendLine (string.Format ("{0}.Appearance = NSAppearance.GetAppearance ({1});", name, NSAppearance.NameDarkAqua.GetType ().FullName));
			//}

			return builder.ToString ();
		}
	}
}
