   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ComboBoxTextBox  �� 
TargetType ��Name  ComboBoxToggleButtonStyle �� 
TargetType �� Property �� Value �Name � ColumnDefinitions F�Name �� Triggers	 P� Property
 P� Value �� 
TargetName �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight K� IsItemsHost-   'System.Windows.Input.KeyboardNavigation    DirectionalNavigation P� 
SourceName �� Triggers�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %Q  &	        &	 a     '�� f    7  7   �� 5      "  ��6.   .R�5   
   �� $ PART_ContentHost��6   |�.  #��z��6=   5      �� 5
      " d�6.   .�5   
   ��  � �� 5      �� 5      "  d�6"   .R�5      � $ mainGrid��6   $��0,0,1,1q�6-   .+�5      � $ uncheckedState��6   .+�5      �� ���	#49FFFFFF$��1,1,0,0q�6=   �� 5      ���	#49000000$��0,0,1,1q�6=   5      � 5      $ checkedState��6   $y�	Collapsed=�65   .+�5      �� ���	#49000000$��1,1,0,0q�6=   �� 5      ���	#49FFFFFF$��0,0,1,1q�6=   5      �� 5      ���	#000000006!   � 5        5!      �� 5"   "   �� 5#   "   $��20 �63   .+�5&      �� $��0��6/   $��Center=�5'   !   $��Center=�5(   !   F� 5*      $ Arrow��5+      $��1��5,      
���   �$��Center=�5.      $��Center=�5/      =����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	50      52      56       59      P� 5:      	 � ��
 . . �5;      �� $ checkedState��6%    � y�$ Visible=�5=   !   �� 5>      $ uncheckedState��6%    � y�$ 	Collapsed=�5@   !   �� 5A      $ mainGrid��6%    � ��$ 1,1,0,0q�5C   !   5D      P� 5F      	 � {�
 .  . �5G      ��  � �� 5H   "   ��5I   &   	#FFADADAD��6A   5K      5L      5O      5Q      �� 5S   
    � ��$ 30��6%   5U      �� 5Y      ComboBoxStyle��" ��6#   .�5Z   
   ��  �@J�SnapsToDevicePixels . 60   �� 5[   
    �@�OverridesDefaultStyle . 62   �� 5\   
    � ��$
 Auto=�6G   �� 5]   
    � ��$
 Auto=�6E   �� 5^   
    � �� . 6:   �� 5_   
    � ��$ 50��6%   �� 5a   
    � �� 5b      �� 5c      "  ��6"   .R�5e      � .+�5f      d� $��ToggleButton��5g      
��� ���#��z��5i      8��  $��2��5k      |�.  $ Press=�5n       5m      �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5o      �� 5q      $��ContentSite��5r       .  #��z 5t      #��z 5u      #��z��5v      $��Center=�5w      $��Center=�5x      �� 5{      $ PART_EditableTextBox��6"   8�� $
��Left=�5~      $��Center=�5      |�. $y�Hidden=�5�      # z 5�      ��5|      Z� 5�      � 5�      $��Popup��5�      $��Bottom=�5�      #��z 5�       . |�.  $��Slide=�5�      .��5�      � $��DropDown��5�       . #��z 5�      #��z 5�      .+�5�   "   �� $ DropDownBorder��5�   %   
�������$��2q�5�   %   
�������5�   %   �� 5�   "   $��4,6,4,6q�6/    . .��5�   &   ��  . $ 	Contained=�6D   5�   #   5�      5�      5�       5�      P� 5�      	 �@��HasItems
 .  . �5�      �� $ DropDownBorder��6%    � ��$ 95��6V   5�      P� 5�      	 �@��
IsGrouping
 . . �5�      ��  � �� .  6N   5�      P� 5�      $ Popup��6"   	 �@�AllowsTransparency
 . . �5�      �� $ DropDownBorder��6%    �@��CornerRadius$ 0��6Y   �� 5�      $ DropDownBorder��6%    � ��$ 0,2,0,0q�6S   5�      P� 5�      	 �@��
IsEditable
 . . �5�      ��  � �� .  6:   �� 5�      $ PART_EditableTextBox��6%    � y�$ Visible=�6`   �� 5�      $ ContentSite��6%    � y�$ Hidden=�6T   5�      5�      5�       5�   
   5�      5�      