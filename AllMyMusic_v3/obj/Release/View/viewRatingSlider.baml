   M S B A M L   `   `   `  ����   
AllMyMusic&     AllMyMusic.View.viewRatingSlider   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ���� *Height+  � 	Resources     starGeometry � Brush � Offset � Color � Geometry B� FillRule E� IsClosed �� Point  borderBrush �� EndPoint  CustomThumbForSlider	 �� 
TargetType
 �� Property �� Value �� 
TargetType  SliderRepeatButtonStyle  sliderStyle ]� DecreaseRepeatButton �� DecreaseLarge ]� IncreaseRepeatButton �� IncreaseLarge ]� Thumb �� Triggers P� Property P� Value �� 
TargetName��%�  &	        &	 +    &	 p    &	 �    &	 s    7  7  7  �  �� .��� $	 0.3Q�$ 
LightCoral��� $ 1Q�$ DarkRed�� B� $ Nonzero=�.��E�  . .���� $
 50,0&��� $ 82,100&��� $
 0,38&��� $ 100,38&��� $ 18,100&��� $
 50,0&�E� .���� $	 0,0&��� $ 100,0&�E� .���� $	 0,0&��� $ 0,100&��� $	 0,1&�.��� $	 0.1Q�$	 Red��� $ 1Q�$ Yellow���� "	 n�.��� 
 �@�OverridesDefaultStyle . �� 
 � �� �� " n�.R��� $��3��$��15��
���   ��� "	 ��.��� 
 �@�OverridesDefaultStyle . �� 
 � �� .  �� 
 � |� .  �� 
 � �� �� " ��.R��� 
������ �� "	 ��.��� 
 � �� �� " ��.R�� .+�]� $��
PART_Track�� �� #��Z 8��   �� #��Z 8��  n� $��Thumb��8�� 
������� P�  �@J�IsMouseOver . . ��� $ Thumb��
 � y�$ Visible=�P�  �@J�IsMouseOver .  . ��� $ Thumb��
 � y�$ Hidden=�.���� -   $��border��$��1q�
�����������  �� ElementName$ ratingSlider�� �� Path$ ForegroundRectColor�._�� .+���  �� Orientation$ 
Horizontal=�.+��� ��H�  H� Drawing# [  �� ��H� # [  �� ��H� # [  �� ��H� # [  �� ��H� # [  �� -   $��hidingRectangle��$��Right=����� $ ratingSlider��$ RemainingValue����� $ ratingSlider��$ ForegroundRectColor��� -   $��slider��#��[  � Maximum$ 10Q� � Minimum$ 0Q� � Value �� $ ratingSlider��$ Value�