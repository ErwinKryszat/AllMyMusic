   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  $    AllMyMusic.Controls.FormRating   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,vwclr-namespace:AllMyMusic.View     -   $��
formRating��  � 	Resources   �� MergedDictionaries �� Source��  �� $+ %../ResourceDictionaries/MainSkin.xaml?��� $3 -../ControlTemplates/RatingSliderTemplate.xaml?�%    .���  � ColumnDefinitions �� ����  �� Path$ 
LabelWidth� �� ElementName$ 
formRating���� $��* �.+��� -   $��label��$��Center=�$��0������ $ Label�$ 
formRating������ $ 
LabelWidth�$ 
formRating������ $ LabelPadding�$ 
formRating������ $% LabelHorizontalContentAlignment�$ 
formRating��"   AllMyMusic.View.RatingSlider  -     Name$ ratingSlider��   RatingSliderTemplate#��[  $��30��$��Center=�$��1��   TagValue �� $ 
formRating��$ Rating�   ForegroundRectColor �� $ 
Background�	 �� RelativeSource	 �� *FindAncestor+
 �� AncestorType"
 ��