   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   SimpleExpanderButtonTemp  �� 
TargetType ��Name �� Padding � ColumnDefinitions F�Name J� RenderTransformOrigin ��Name �� Triggers P� Property	 P� Value
 �� 
TargetName �� Property �� Value  SimpleExpanderTemp d�Name � OverridesDefaultStyle d� 	IsChecked �� Path �� RelativeSource  AnimatedExpanderButtonTemp �� Angle N� EnterActions �� 
TargetName �� TargetProperty	 W� To g� Duration N� ExitActions  StretchyExpanderTemp � LayoutTransform �� ScaleY'    !AllMyMusic.View.MultiplyConverter  multiplyConverter  RevealExpanderTemp ��Name
 � Tag `� 	Converter �� ElementName  AnimatedExpanderButtonWidth  RevealExpanderWidth  AdjustExpanderWidth 	 AdjustExpanderWidthRL�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      $vwclr-namespace:AllMyMusic.View  5      .sys&clr-namespace:System;assembly=mscorlib  5      %�!  &	        &	 !    7   &	 �    &	 _    7  &	 �    &	 �    7  7  &	      &	 �    7  &	 A    &		 \    7  �� 5      "  d�67   .R�5	   
   �� $ ExpanderButtonBorder��6   #��z��5
      #��z��5      #��z��5      # z��5      ._�5      �  5      �� 5      $
��Auto �6'   �� 5      $��* �6'   .+�5      �� 
������ $��2��5       A� 5      $��Circle��6   $��0��5      
�������$��20��5      $��20��5      $��Center=�5      $��Center=�5      F� 5      $
 Sign��6   $��0��5      +����    @KL р�� @KL �@KL     �@KL ��� 	���	#FF666666$��10��5"      $��10��5#      $��2��5$      $��Center=�5%      $��Center=�5&      $ 0.5,0.5&�5'      �� 5)      $ HeaderContent��6#   $��1��5*   #   $��4,0,0,0q�5+   #   $��Content��5,   #   5-      5.       5/   
   P� 51       �@d�	IsChecked$
	 TrueY�52      . �53      �� $

 Sign��6)    � �� ��    @KL р�� @KL 	6;   54      P� 57       �@J�IsMouseOver	 . . �59      �� $
 Circle��5;   %    � �� �	#FF3C7FB1�� 5<      $

 Sign��5>   %    � �� �#2225?      P� 5@       � ��	 . . �5B      �� $
 Circle��5D   %    � �� �	#FF526C7B�� 5E      $
 Circle��5G   %    � ��$	 1.5��5F   %   5G   %   �� 5H      $

 Sign��5J   %    � �� �	#FF0033665K      5M      �� 5P      "  9�61   .R�5Q   
   ]� .+�5R      d� $ ExpanderButton��6   $	��Top=�5S      8��  #��z��5U       . $��1.5,0q�5X       5V      �� $ 
IsExpanded� �� *TemplatedParent+5Y      �� 5Z      $ ExpanderContent��6   $y�	Collapsed=�5\   #   $��Bottom=�5]   #   5^       5_   
   P� 5`       �@9�
IsExpanded	 . . �5a      �� $
 ExpanderContent��6    � y�$ Visible=�6L   5b      5d      �� 5g      "  d�69   .R�5h   
   �� $ ExpanderButtonBorder��6   #��z��5i      #��z��5j      #��z��5k      # z��5l      ._�5n      �  5o      �� 5p      $
��Auto �6'   �� 5q      $��* �6'   .+�5s      �� 
������ $��2��5t       A� 5u      $��Circle��6   $��0��5v      
�������$��20��5x      $��20��5y      $��Center=�5z      $��Center=�5{      F� 5}      $ Arrow��6   $��0��5~      "����@B `� � �D @KL � z `� 	���	#FF666666$��2��5�      $��Center=�5�      $��Center=�5�      $ 0.5,0.5&�5�      z�5�      �� 5�      $ 0Q�6*   5�      �� 5�      $ HeaderContent��6#   $��1��5�   '   $��4,0,0,0q�5�   '   $��Content��5�   '   5�      5�       5�   
   P� 5�       �@d�	IsChecked$
	 TrueY�5�       5�      �� 5�      .��5�      �� .��5�      W� $ Arrow��6.   $4 .(Path.RenderTransform).(RotateTransform.Angle)�5�   2   $	 180X�5�   2   $ 0:0:0.4D�5�   2   5�      5�       5�      �� 5�      .��5�      �� .��5�      W� $ Arrow��6.   $4 .(Path.RenderTransform).(RotateTransform.Angle)�5�   2   $ 0X�5�   2   $ 0:0:0.4D�5�   2   5�      5�      5�      P� 5�       �@J�IsMouseOver	 . . �5�      �� $
 Circle��5�   %    � �� �	#FF3C7FB1�� 5�      $
 Arrow��5�   %    � �� �#2225�      P� 5�       � ��	 . . �5�      �� $
 Circle��5�   %    � �� �	#FF526C7B�� 5�      $
 Circle��5�   %    � ��$	 1.5��5�   %   5�   %   �� 5�      $
 Arrow��5�   %    � �� �	#FF0033665�      5�      �� 5�      "  9�63   .R�5�   
   ]� .+�5�      d� $ ExpanderButton��6   $	��Top=�5�      8��  #��z��5�       . $��1.5,0q�5�       5�      �� $ 
IsExpanded� �� *TemplatedParent+5�      �� 5�      $ ExpanderContent��6   $��Content��5�   #   $��Bottom=�5�   #    5�      �� 5�      $ 0Q�6%   5�      5�       5�   
   P� 5�       �@9�
IsExpanded	 .  5�      �� 5�      .��5�      �� .��5�      W� $ ExpanderContent��6.   $@ :(ContentPresenter.LayoutTransform).(ScaleTransform.ScaleY)�5�   2   $ 1X�5�   2   $ 0:0:0.4D�5�   2   5�      5�       5�      �� 5�      .��5�      �� .��5�      W� $ ExpanderContent��6.   $@ :(ContentPresenter.LayoutTransform).(ScaleTransform.ScaleY)�5�   2   $ 0X�5�   2   $ 0:0:0.4D�5�   2   5�      5�      5�      5�         5�      6   �� 5�      "  9�61   .R�5�   
   ]� .+�5�      d� $ ExpanderButton��6   $	��Top=�5�      8��  #��z��5�       . $��1.5,0q�5      5�      �� $ 
IsExpanded� �� *TemplatedParent+5     �� 5     $ ExpanderContentScrollView��6   $��Bottom=�6>   $��Hidden=�5     $��Hidden=�5     $��Stretch=�5     $��Bottom=�5      5	     X�5
     0.0Q�6&   ��5     `� 5     8  .1�5     �� $ ActualHeight�6"   $ ExpanderContent��66   �� 5     $	 Tag�6"    6-   �� *Self+5     .��5     �� $ ExpanderContent��6#   $��Content��6<   5     5      5  
   P� 5      �@9�
IsExpanded	 .  5     �� 5     .��5     �� .��5     W� $ ExpanderContentScrollView��6.   $	 Tag�5  2   $ 1X�5  2   $ 0:0:0.4D�5  2   5     5      5!     �� 5"     .��5#     �� .��5$     W� $ ExpanderContentScrollView��6.   $	 Tag�5%  2   $ 0X�5&  2   $ 0:0:0.4D�5'  2   5(     5)     5+     5-     �� 51     "  d�6:   .R�52  
   �� $ ExpanderButtonBorder��6   #��z��53     #��z��54     #��z��55     # z��56     #��z��57     #��z��58     ._�5:     � .+�5;     �� 
������ 6   A� 5=     $��Circle��6   
�������$��20��5?     $��20��5@     $��Center=�5A     $��Center=�5B     F� 5D     $ Arrow��6   "����@B `� � z @KL �@B  �� 	���	#FF666666$��2��5G     $��Center=�5H     $��Center=�5I     $ 0.5,0.5&�5J     z�5K     �� 5L     $ 0Q�6*   5N     5Q     5R      5S  
   P� 5U      �@d�	IsChecked$
	 TrueY�6+    5W     �� 5X     .��5Y     �� .��5Z     W� $ Arrow��6.   $4 .(Path.RenderTransform).(RotateTransform.Angle)�5[  2   $	 180X�5\  2   $ 0:0:0.4D�5]  2   5^     5_      5a     �� 5b     .��5c     �� .��5d     W� $ Arrow��6.   $4 .(Path.RenderTransform).(RotateTransform.Angle)�5e  2   $ 0X�5f  2   $ 0:0:0.4D�5g  2   5h     5i     5k     P� 5n      �@J�IsMouseOver	 . . �5o     �� $
 Circle��6=    � �� �	#FF3C7FB1�� 5p     $
 Arrow��6=    � �� �	#FF3C7FB15q     P� 5r      � ��	 . . �5s     �� $
 Circle��6>    � �� �	#FF526C7B�� 5t     $
 Circle��6B    � ��$	 1.5��65   6B   �� 5u     $
 Arrow��6=    � �� �	#FF0033665v     5x     �� 5~     "  9�62   .R�5  
   ]� .+�5�     �� $ ExpanderContentScrollView��6   $��Bottom=�6>   $��Hidden=�5�     $��Hidden=�5�     $��Stretch=�5�     $��Bottom=�5�      5�     X�5�     0.0Q�6&   ��5�     `� 5�     8   .1�5�     �� $ ActualWidth�6"   $ ExpanderContent��65   �� 5�     $	 Tag�6"    6-   �� *Self+5�     .��5�     �� $ ExpanderContent��6#   $��Content��6<   5�     5�      5�  
   P� 5�      �@9�
IsExpanded	 .  5�     �� 5�     .��5�     �� .��5�     W� $ ExpanderContentScrollView��6.   $	 Tag�5�  2   $ 1X�5�  2   $ 0:0:0.4D�5�  2   5�     5�      5�     �� 5�     .��5�     �� .��5�     W� $ ExpanderContentScrollView��6.   $	 Tag�5�  2   $ 0X�5�  2   $ 0:0:0.4D�5�  2   5�     5�     5�     5�     �� 5�     "  9�62   .R�5�  
   ]� .+�5�     �� $ ExpanderContent��6   $y�	Collapsed=�5�  #   $��Bottom=�5�  #   5�      5�  
   P� 5�      �@9�
IsExpanded	 . . �5�     �� $
 ExpanderContent��6    � y�$ Visible=�6L   5�     5�     �� 5�     "  9�64   .R�5�  
   ]� .+�5�     d� $ ExpanderButton��6   $	��Top=�5�     $��Right=�5�     8��  #��z��5�      . $��1.5,0q�5�      5�     �� $ 
IsExpanded� �� *TemplatedParent+5�     �� 5�     $ ExpanderContent��6   $y�	Collapsed=�5�  #   $��Bottom=�5�  #   5�      5�  
   P� 5�      �@9�
IsExpanded	 . . �5�     �� $
 ExpanderContent��6    � y�$ Visible=�6L   5�     5�     5�     