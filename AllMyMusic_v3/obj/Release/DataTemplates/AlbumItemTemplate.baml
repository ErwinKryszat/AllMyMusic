   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source !  booleanToVisibilityConverter1  AlbumItemTemplate ��Name �� 
TargetType �� Triggers �� Binding �� Path �� Value �� Property	 �� Value
 �� EndPoint �� 
StartPoint � Color � Offset  AlbumItemBackground ��Name �� RelativeSource �� AncestorType#    AllMyMusic.View.viewAlbumList  	TextBrush �� ElementName �� 	Converter�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     $vwclr-namespace:AllMyMusic.View    �� $+ %../ResourceDictionaries/MainSkin.xaml?�%B  &	        &	      7  7  7   7  �� �� .R��� $ ItemBackground������ " �� ��  �� $ 
IsSelected� True.a���  � ��	 �� $	
 0,1&�$	 0,0&�.��� $ 	#FFF3F3F3��$ 1Q�� $ 	LawnGreen��$ 0.307Q�._�� 8��  $��10q�.+��� ���	#49FFFFFF$��2,2,0,0q��� ���	#49000000$��0,0,2,2q��� .+��� $ 
albumCover��$��5q����� $ 
StampImage����� $  DataContext.StampImageSize� �� "   ���� $  DataContext.StampImageSize� �� "   �� $
��Wrap=�8�� $��5,0,0,0q�$��Normal"�$��16(����� $ BandName����� $ 
albumCover��$ ActualWidth�y��� $# DataContext.BandNameVisbility�8   �� "   �� $
��Wrap=�8�� $��Normal"�$��16(�$��5,0,0,0q����� $ YearAndTitle����� $ 
albumCover��$ ActualWidth�