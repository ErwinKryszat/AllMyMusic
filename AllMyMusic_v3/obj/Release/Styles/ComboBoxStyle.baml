   M S B A M L   `   `   `  ���� \  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ComboBoxTextBox  �� 
TargetType ��Name  ComboBoxToggleButtonStyle �� 
TargetType �� Property �� Value �Name � ColumnDefinitions F�Name �� Triggers	 P� Property
 P� Value �� 
TargetName �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight K� IsItemsHost-    'System.Windows.Input.KeyboardNavigation    DirectionalNavigation P� 
SourceName �� Triggers�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      %!  &	        &	 7     '�� -    7  7   �� "  ��.R��� $ PART_ContentHost��|�.  #��z���� " d�.���  � �� �� "  d�.R�� $ mainGrid��$��0,0,1,1q�.+�� $ uncheckedState��.+��� ���	#49FFFFFF$��1,1,0,0q��� ���	#49000000$��0,0,1,1q�� $ checkedState��$y�	Collapsed=�.+��� ���	#49000000$��1,1,0,0q��� ���	#49FFFFFF$��0,0,1,1q��� ���	#00000000�  �� �� $��20 �.+��� $��0��$��Center=�$��Center=�F� $ Arrow��$��1��
���   �$��Center=�$��Center=�=����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	 P� 	 � ��
 . . ��� $ checkedState�� � y�$ Visible=��� $ uncheckedState�� � y�$ 	Collapsed=��� $ mainGrid�� � ��$ 1,1,0,0q�P� 	 � {�
 .  . ���  � �� ��	#FFADADAD����  � ��$ 30���� ComboBoxStyle��" ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  � ��$ 50����  � �� �� "  ��.R�� .+�d� $��ToggleButton��
��� ���#��z��8��  $��2��|�.  $ Press=� �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+�� $��ContentSite�� .  #��z #��z #��z��$��Center=�$��Center=��� $ PART_EditableTextBox��8�� $
��Left=�$��Center=�|�. $y�Hidden=�# z ��Z� � $��Popup��$��Bottom=�#��z  . |�.  $��Slide=�.��� $��DropDown�� . #��z #��z .+��� $ DropDownBorder��
�������$��2q�
��������� $��4,6,4,6q� . .����  . $ 	Contained=� P� 	 �@��HasItems
 .  . ��� $ DropDownBorder�� � ��$ 95��P� 	 �@��
IsGrouping
 . . ���  � �� .  P� $ Popup��	 �@�AllowsTransparency
 . . ��� $ DropDownBorder�� �@��CornerRadius$ 0���� $ DropDownBorder�� � ��$ 0,2,0,0q�P� 	 �@��
IsEditable
 . . ���  � �� .  �� $ PART_EditableTextBox�� � y�$ Visible=��� $ ContentSite�� � y�$ Hidden=� 