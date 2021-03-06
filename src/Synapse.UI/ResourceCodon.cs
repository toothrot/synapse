//
// ResourceCodon.cs
// 
// Copyright (C) 2009 Eric Butler
//
// Authors:
//   Eric Butler <eric@extremeboredom.net>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using Mono.Addins;

namespace Synapse.UI
{
	public class ResourceCodon : TypeExtensionNode
	{
		[NodeAttribute("name", "Name of resource")]
		string m_Name;

		public string Name {
			get {
				return m_Name;
			}
		}

		public string GetResourceString ()
		{
			using (StreamReader reader = new StreamReader(Addin.GetResource(m_Name))) {
				return reader.ReadToEnd();
			}
		}
	}
}
