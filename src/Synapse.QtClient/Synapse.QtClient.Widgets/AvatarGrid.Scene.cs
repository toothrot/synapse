//
// AvatarGrid.Scene.cs
// 
// Copyright (C) 2009 Eric Butler
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
//

using System;
using Synapse.UI;
using Qyoto;

namespace Synapse.QtClient.Widgets
{
	public partial class AvatarGrid<T> : QGraphicsView
	{
		class AvatarGridScene : QGraphicsScene
		{
			AvatarGrid<T> m_Grid;

			QGraphicsLineItem m_GroupDropIndicatorItem;
			RosterItemGroup   m_GroupDropBeforeGroup;
				
			public AvatarGridScene (AvatarGrid<T> grid) : base (grid)
			{
				m_Grid = grid;

				m_GroupDropIndicatorItem = new QGraphicsLineItem(null, this);
				m_GroupDropIndicatorItem.SetPen(new QPen(new QBrush(Qt.GlobalColor.red), 1));
				m_GroupDropIndicatorItem.SetVisible(false);
				m_GroupDropIndicatorItem.SetZValue(500);
			}

			protected override void DragMoveEvent (Qyoto.QGraphicsSceneDragDropEvent arg1)
			{
				if (arg1.MimeData() is RosterItemGroupMimeData) {
					arg1.Accept();
					
					m_Grid.AllGroupsCollapsed = true;

					var pos = arg1.ScenePos();

					m_GroupDropBeforeGroup = null;
					foreach (var group in m_Grid.SortedGroups) {
						if (group.IsVisible()) {
							if (pos.Y() < (group.Y() + (group.BoundingRect().Height() / 2))) {
								m_GroupDropBeforeGroup = group;
								break;
							}
						}
					}

					if (m_GroupDropBeforeGroup != null) {
						m_GroupDropIndicatorItem.SetLine(0, 0, base.Width(), 0);
						m_GroupDropIndicatorItem.SetPos(0, m_GroupDropBeforeGroup.Y() - (m_Grid.IconPadding / 2));
						m_GroupDropIndicatorItem.SetVisible(true);
					} else {
						m_GroupDropIndicatorItem.SetVisible(false);
					}
				} else if (arg1.MimeData() is RosterItemMimeData<T>) {
					m_Grid.AllGroupsCollapsed = true;
					base.DragMoveEvent(arg1);
				} else {
					base.DragMoveEvent(arg1);
				}
			}
	
			protected override void DropEvent (Qyoto.QGraphicsSceneDragDropEvent arg1)
			{
				if (arg1.MimeData() is RosterItemGroupMimeData) {
					var mime = (RosterItemGroupMimeData)arg1.MimeData();
					if (mime.Group != m_GroupDropBeforeGroup && m_GroupDropBeforeGroup != null) {
						int newOrder = m_Grid.Model.GetGroupOrder(m_GroupDropBeforeGroup.Name) - 1;
						var editableModel = (IAvatarGridEditableModel<T>)m_Grid.Model;
						editableModel.SetGroupOrder(mime.Group.Name, newOrder);
						Console.WriteLine("Setting group order to: " + newOrder);
						m_Grid.ResizeAndRepositionGroups();
					}
					
					m_GroupDropIndicatorItem.SetVisible(false);

					m_Grid.AllGroupsCollapsed = false;
				} else if (arg1.MimeData() is RosterItemMimeData<T>) {
					m_Grid.AllGroupsCollapsed = false;
					base.DropEvent(arg1);
				} else {
					base.DropEvent(arg1);
				}
			}
			
			protected override void DragLeaveEvent (Qyoto.QGraphicsSceneDragDropEvent arg1)
			{
				if (arg1.MimeData() is RosterItemGroupMimeData) {
					m_GroupDropIndicatorItem.SetVisible(false);
					m_Grid.AllGroupsCollapsed = false;
				} else if (arg1.MimeData() is RosterItemMimeData<T>) {
					m_Grid.AllGroupsCollapsed = false;
					base.DragLeaveEvent(arg1);
				} else {
					base.DragLeaveEvent(arg1);
				}
			}
		}
	}
}
