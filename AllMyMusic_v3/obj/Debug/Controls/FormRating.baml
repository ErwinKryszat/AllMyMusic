   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      $    AllMyMusic.Controls.FormRating   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,vwclr-namespace:AllMyMusic.View     5      -   5        $��
formRating��5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $3 -../ControlTemplates/RatingSliderTemplate.xaml?�6%   %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      ��6   ��  �� Path$ 
LabelWidth� �� ElementName$ 
formRating���� 5      $��* �6   .+�5   
   �� -   5        $��label��5      $��Center=�5      $��0��5       ��5      �� $ Label�$ 
formRating����5      �� $ 
LabelWidth�$ 
formRating����5      �� $ LabelPadding�$ 
formRating����5      �� $% LabelHorizontalContentAlignment�$ 
formRating��5       "   AllMyMusic.View.RatingSlider  5#   
   -   5          Name$ ratingSlider��5#         RatingSliderTemplate#��[  5$      $��30��5%      $��Center=�5&      $��1��5'         TagValue 5(      �� $ 
formRating��$ Rating�   ForegroundRectColor 5)      �� $ 
Background�	 �� RelativeSource	 �� *FindAncestor+
 �� AncestorType"
 ��5,      5-      