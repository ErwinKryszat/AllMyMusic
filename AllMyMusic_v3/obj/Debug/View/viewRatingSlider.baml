   M S B A M L   `   `   `  ����  
AllMyMusic&     AllMyMusic.View.viewRatingSlider   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ��5      �� *Height+  � 	Resources  5         starGeometry � Brush � Offset � Color � Geometry B� FillRule E� IsClosed �� Point  borderBrush �� EndPoint  CustomThumbForSlider	 �� 
TargetType
 �� Property �� Value �� 
TargetType  SliderRepeatButtonStyle  sliderStyle ]� DecreaseRepeatButton �� DecreaseLarge ]� IncreaseRepeatButton �� IncreaseLarge ]� Thumb �� Triggers P� Property P� Value �� 
TargetName��%�  &	        &	 h    &	 �    &	 �    &	 	    7  7  7  � 5   
    5      �� 5      .��5      � $	 0.3Q�6#   $ 
LightCoral��5   #   � 5      $ 1Q�6#   $ DarkRed��5   #   5       5      B� 5      $ Nonzero=�6   .��5      E�  . .��5      �� $
 50,0&�6&   �� 5      $ 82,100&�6&   �� 5      $
 0,38&�6&   �� 5       $ 100,38&�6&   �� 5!      $ 18,100&�6&   �� 5"      $
 50,0&�6&   5#      E� 5%      .��5&      �� $	 0,0&�6&   �� 5'      $ 100,0&�6&   5(      E� 5*      .��5+      �� $	 0,0&�6&   �� 5,      $ 0,100&�6&   5-      5/      51      �� 53   
   $	 0,1&�54      .��55      � $	 0.1Q�6   $	 Red��56      � 57      $ 1Q�6   $ Yellow��58      59      �� 5;   
   "	 n�5<      .�5=      �� 
 �@�OverridesDefaultStyle . 5>      �� 5?      
 � �� 5@      �� 5A      " n�6&   .R�5B      �� $��3��6$   $��15��5C   $   
���   �5D   $   5E      5G      5H      �� 5J   
   "	 ��5K      .�5L      �� 
 �@�OverridesDefaultStyle . 5M      �� 5N      
 � �� .  5O      �� 5P      
 � |� .  5Q      �� 5R      
 � �� 5S      �� 5T      " ��6&   .R�5U      �� 
������ 6$   5V      5X      5Y      �� 5[   
   "	 ��6$   .�5\      �� 
 � �� 5]      �� 5^      " ��6&   .R�5_      � .+�5`      ]� $��
PART_Track��6$    5b   "   �� 5c   &   #��Z 63   8��  5d   7    5g   "   �� 5h   &   #��Z 63   8�� 5i   7    5l   "   n� 5m   &   $��Thumb��6,   8�� 
�������5o   0   5q      5s       5t      P� 5u       �@J�IsMouseOver . . �5v   "   �� $ Thumb��6)   
 � y�$ Visible=�6R   5w      P� 5y       �@J�IsMouseOver .  . �5z   "   �� $ Thumb��6)   
 � y�$ Hidden=�6R   5{      5}      5      5�      5�      .��5�      �� -   5        $��border��5�      $��1q�6   
���������6B   ��  �� ElementName$ ratingSlider�� �� Path$ ForegroundRectColor�._�5�   
   � .+�5�      ��  �� Orientation$ 
Horizontal=�6   .+�5�      �� ��5�      H� 5�       H� Drawing# [  6'   5�      �� 5�      ��5�      H� 5�      # [  6'   5�      �� 5�      ��5�      H� 5�      # [  6'   5�      �� 5�      ��5�      H� 5�      # [  6'   5�      �� 5�      ��5�      H� 5�      # [  6'   5�      5�      �� 5�      -   5        $��hidingRectangle��5�      $��Right=�5�      ��5�      �� $ ratingSlider��$ RemainingValue���5�      �� $ ratingSlider��$ ForegroundRectColor��� 5�      -   5        $��slider��5�      #��[ 5�       � Maximum$ 10Q�5�       � Minimum$ 0Q�5�       � Value 5�      �� $ ratingSlider��$ Value�5�      5�      5�      5�      