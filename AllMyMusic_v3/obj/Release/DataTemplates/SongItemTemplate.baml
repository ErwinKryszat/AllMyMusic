   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   SongItemTemplate ��Name �� 
TargetType �� Triggers �� Binding �� Path �� Value �� Property	 �� Value
 �� EndPoint �� 
StartPoint � Color � Offset  AlbumItemBackground � ColumnDefinitions  NoChromeButtonStyle �� RelativeSource �� AncestorType"    AllMyMusic.View.viewPlayList  	TextBrush�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     $vwclr-namespace:AllMyMusic.View    �� $+ %../ResourceDictionaries/MainSkin.xaml?��� $( "../Styles/NoChromeButtonStyle.xaml?�%�  &	        7  7  7  7  7  7  7  7  �� .R��� $ ItemBackground������ " �� ��  �� $ 
IsSelected� True.a���  � ��	 �� $	
 0,1&�$	 0,0&�.��� $ 	#FFF3F3F3��$ 1Q�� $ Yellow��$ 0.307Q���  �� $ IsNowPlaying� True.a���  � ��
	 � �|�._�� 8��  $��5q� �� $
��Auto ��� $��* ��� $
��Auto �.+��� ���	#49FFFFFF$��2,2,0,0q��� ���	#49000000$��0,0,2,2q��� 8�� $��0��$��5q�$��50��$��50������ $# DataContext.CoverImageCommand� �� "   ���� .���� ���� $ 
StampImage��� $��1��$��5,0,0,0q�.+��� 8�� $
��Bold"�$��16(����� $ 	SongTitle��� 8�� ���� $ BandName��� 8�� ���� $ 	AlbumName��� $��2��$��0,0,5,0q�.+��� $��Right=�8�� ���� $ Track��� $��Right=�8�� ���� $
 Year��� $��Right=�8�� ���� $ Genre�