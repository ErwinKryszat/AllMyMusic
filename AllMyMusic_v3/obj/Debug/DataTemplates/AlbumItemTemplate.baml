   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source !  booleanToVisibilityConverter1  AlbumItemTemplate ��Name �� 
TargetType �� Triggers �� Binding �� Path �� Value �� Property	 �� Value
 �� EndPoint �� 
StartPoint � Color � Offset  AlbumItemBackground ��Name �� RelativeSource �� AncestorType#    AllMyMusic.View.viewAlbumList  	TextBrush �� ElementName �� 	Converter�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      $vwclr-namespace:AllMyMusic.View  5        5      �� 5   
   $+ %../ResourceDictionaries/MainSkin.xaml?�6   %  &	        &	      7  7  7   7  �� 5
      6#   �� 5      .R�5   
   �� $ ItemBackground��6   ��5      �� 5      " ��6    5      �� 5       6&   �� $ 
IsSelected� True6J   .a�5      ��  � ��	 5   "   �� 5   &   $	
 0,1&�6:   $	 0,0&�6I   .��5   *   � $ 	#FFF3F3F3��67   $ 1Q�6I   � 5   *   $ 	LawnGreen��67   $ 0.307Q�6I   5   '   5      5      5       ._�5#      � 8��  $��10q�6E   .+�5$      �� ���	#49FFFFFF$��2,2,0,0q�61   �� 5%      ���	#49000000$��0,0,2,2q�61   �� 5'      .+�5*      �� $ 
albumCover��6   $��5q�5,       ��5+       �� $ 
StampImage���5-       �� $  DataContext.StampImageSize� �� "   ��5.       �� $  DataContext.StampImageSize� �� "   �� 50      $
��Wrap=�51      8�� $��5,0,0,0q�52      $��Normal"�53      $��16(�54      ��50       �� $ BandName���55      �� $ 
albumCover��$ ActualWidth�y�56      �� $# DataContext.BandNameVisbility�8   �� "   �� 59      $
��Wrap=�5:      8�� $��Normal"�5;      $��16(�5<      $��5,0,0,0q�5=      ��59       �� $ YearAndTitle���5=   -   �� $ 
albumCover��$ ActualWidth�5?      5A      5C      5E      5G      