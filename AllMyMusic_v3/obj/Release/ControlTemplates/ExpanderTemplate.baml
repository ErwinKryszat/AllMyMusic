   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   SimpleExpanderButtonTemp  �� 
TargetType ��Name �� Padding � ColumnDefinitions F�Name J� RenderTransformOrigin ��Name �� Triggers P� Property	 P� Value
 �� 
TargetName �� Property �� Value  SimpleExpanderTemp d�Name � OverridesDefaultStyle d� 	IsChecked �� Path �� RelativeSource  AnimatedExpanderButtonTemp �� Angle N� EnterActions �� 
TargetName �� TargetProperty	 W� To g� Duration N� ExitActions  StretchyExpanderTemp � LayoutTransform �� ScaleY'    !AllMyMusic.View.MultiplyConverter  multiplyConverter  RevealExpanderTemp ��Name
 � Tag `� 	Converter �� ElementName  AnimatedExpanderButtonWidth  RevealExpanderWidth  AdjustExpanderWidth 	 AdjustExpanderWidthRL�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     $vwclr-namespace:AllMyMusic.View  .sys&clr-namespace:System;assembly=mscorlib  %�  &	        &	     7   &	 ,    &	 �    7  &	 �	    &	 �	    7  7  &	 R    &	 �    7  &	 �    &		 4    7  �� "  d�.R��� $ ExpanderButtonBorder��#��z��#��z��#��z��# z��._��  �� $
��Auto ��� $��* �.+��� 
������ $��2��A� $��Circle��$��0��
�������$��20��$��20��$��Center=�$��Center=�F� $
 Sign��$��0��+����    @KL р�� @KL �@KL     �@KL ��� 	���	#FF666666$��10��$��10��$��2��$��Center=�$��Center=�$ 0.5,0.5&��� $ HeaderContent��$��1��$��4,0,0,0q�$��Content�� P�  �@d�	IsChecked$
	 TrueY�. ��� $

 Sign�� � �� ��    @KL р�� @KL 	P�  �@J�IsMouseOver	 . . ��� $
 Circle�� � �� �	#FF3C7FB1�� $

 Sign�� � �� �#222P�  � ��	 . . ��� $
 Circle�� � �� �	#FF526C7B�� $
 Circle�� � ��$	 1.5���� $

 Sign�� � �� �	#FF003366�� "  9�.R�]� .+�d� $ ExpanderButton��$	��Top=�8��  #��z�� . $��1.5,0q� �� $ 
IsExpanded� �� *TemplatedParent+�� $ ExpanderContent��$y�	Collapsed=�$��Bottom=� P�  �@9�
IsExpanded	 . . ��� $
 ExpanderContent�� � y�$ Visible=��� "  d�.R��� $ ExpanderButtonBorder��#��z��#��z��#��z��# z��._��  �� $
��Auto ��� $��* �.+��� 
������ $��2��A� $��Circle��$��0��
�������$��20��$��20��$��Center=�$��Center=�F� $ Arrow��$��0��"����@B `� � �D @KL � z `� 	���	#FF666666$��2��$��Center=�$��Center=�$ 0.5,0.5&�z��� $ 0Q��� $ HeaderContent��$��1��$��4,0,0,0q�$��Content�� P�  �@d�	IsChecked$
	 TrueY� �� .���� .��W� $ Arrow��$4 .(Path.RenderTransform).(RotateTransform.Angle)�$	 180X�$ 0:0:0.4D� �� .���� .��W� $ Arrow��$4 .(Path.RenderTransform).(RotateTransform.Angle)�$ 0X�$ 0:0:0.4D�P�  �@J�IsMouseOver	 . . ��� $
 Circle�� � �� �	#FF3C7FB1�� $
 Arrow�� � �� �#222P�  � ��	 . . ��� $
 Circle�� � �� �	#FF526C7B�� $
 Circle�� � ��$	 1.5���� $
 Arrow�� � �� �	#FF003366�� "  9�.R�]� .+�d� $ ExpanderButton��$	��Top=�8��  #��z�� . $��1.5,0q� �� $ 
IsExpanded� �� *TemplatedParent+�� $ ExpanderContent��$��Content��$��Bottom=� �� $ 0Q� P�  �@9�
IsExpanded	 .  �� .���� .��W� $ ExpanderContent��$@ :(ContentPresenter.LayoutTransform).(ScaleTransform.ScaleY)�$ 1X�$ 0:0:0.4D� �� .���� .��W� $ ExpanderContent��$@ :(ContentPresenter.LayoutTransform).(ScaleTransform.ScaleY)�$ 0X�$ 0:0:0.4D�   �� "  9�.R�]� .+�d� $ ExpanderButton��$	��Top=�8��  #��z�� . $��1.5,0q� �� $ 
IsExpanded� �� *TemplatedParent+�� $ ExpanderContentScrollView��$��Bottom=�$��Hidden=�$��Hidden=�$��Stretch=�$��Bottom=� X�0.0Q���`� 8  .1��� $ ActualHeight�$ ExpanderContent���� $	 Tag� �� *Self+.���� $ ExpanderContent��$��Content�� P�  �@9�
IsExpanded	 .  �� .���� .��W� $ ExpanderContentScrollView��$	 Tag�$ 1X�$ 0:0:0.4D� �� .���� .��W� $ ExpanderContentScrollView��$	 Tag�$ 0X�$ 0:0:0.4D��� "  d�.R��� $ ExpanderButtonBorder��#��z��#��z��#��z��# z��#��z��#��z��._�� .+��� 
������ A� $��Circle��
�������$��20��$��20��$��Center=�$��Center=�F� $ Arrow��"����@B `� � z @KL �@B  �� 	���	#FF666666$��2��$��Center=�$��Center=�$ 0.5,0.5&�z��� $ 0Q� P�  �@d�	IsChecked$
	 TrueY� �� .���� .��W� $ Arrow��$4 .(Path.RenderTransform).(RotateTransform.Angle)�$	 180X�$ 0:0:0.4D� �� .���� .��W� $ Arrow��$4 .(Path.RenderTransform).(RotateTransform.Angle)�$ 0X�$ 0:0:0.4D�P�  �@J�IsMouseOver	 . . ��� $
 Circle�� � �� �	#FF3C7FB1�� $
 Arrow�� � �� �	#FF3C7FB1P�  � ��	 . . ��� $
 Circle�� � �� �	#FF526C7B�� $
 Circle�� � ��$	 1.5���� $
 Arrow�� � �� �	#FF003366�� "  9�.R�]� .+��� $ ExpanderContentScrollView��$��Bottom=�$��Hidden=�$��Hidden=�$��Stretch=�$��Bottom=� X�0.0Q���`� 8   .1��� $ ActualWidth�$ ExpanderContent���� $	 Tag� �� *Self+.���� $ ExpanderContent��$��Content�� P�  �@9�
IsExpanded	 .  �� .���� .��W� $ ExpanderContentScrollView��$	 Tag�$ 1X�$ 0:0:0.4D� �� .���� .��W� $ ExpanderContentScrollView��$	 Tag�$ 0X�$ 0:0:0.4D��� "  9�.R�]� .+��� $ ExpanderContent��$y�	Collapsed=�$��Bottom=� P�  �@9�
IsExpanded	 . . ��� $
 ExpanderContent�� � y�$ Visible=��� "  9�.R�]� .+�d� $ ExpanderButton��$	��Top=�$��Right=�8��  #��z�� . $��1.5,0q� �� $ 
IsExpanded� �� *TemplatedParent+�� $ ExpanderContent��$y�	Collapsed=�$��Bottom=� P�  �@9�
IsExpanded	 . . ��� $
 ExpanderContent�� � y�$ Visible=�