   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullG(clr-namespace:VCheckViewer.Views.WindowsVCheckViewer.Views.Windows  5   	   ,    &VCheckViewer.Views.Windows.LoginWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:VCheckViewer.Views.Windows     5   	     � WindowState$  	Maximized=�5      $	��750��62   $
��1200��6B    � Title$ VCheck Viewer��5   	   .��5	      � .+�5
   
    � -   5          �Name$ 
LoginFrame��5
      �� 5   
   -   5         ��Name$ PopupBackground��5      ._�5      � -   5        $��popup��5      $��False��6!   $��Center=�60    � AllowsTransparency$
 True��6C   .��5      ��  &  "Themes_PopupMessageFrameBackground#���   6   #���   6[   $��0q�6�   $	��400��6�   $	��250��6�    �� CornerRadius$ 20��6�   ._�5      �  � RowDefinitions 5      �� 5      $��2* �6,   �� 5      $��* �6,   .+�5      �� $��0��6!   ._�5      �� -   5         ��Name$ PopupContent��5   (   $
��Wrap=�6>    �� TextAlignment$ Center=�6R   $��
40,40,40,0q�6i   $��15(�6}      Themes_PopupMessageFontColor#���  6�   5      �� 5      $��1��6!   $��Center=�6.   $��0,0,0,40q�6K   ._�5      �� 	 �� Orientation$	 
Horizontal=�6)   .+�5   "   �� -   5        
 ��Name$
 CancelButton��5   )   $��No��6?   $��0,0,10,0q�6g     RoundButton#���  6y   $��40��6�   $	��100��6�   #���  6�   #���  6�   
������ 60  �� 5   "   -   5        $
 ContinueButton��5   )   $	��Yes��6A   $��10,0,0,0q�6l   #���  6~   $��40��6�   $	��100��6�     Themes_ButtonColor#���  6�   #���  6�   #���  6!  �� 5   "   -   5        $
 OKButton��5   )   $��OK��6;   $��10,0,0,0q�6c   #���  6u   $��40��6�   $	��100��6�   #���  6�   #���  6�   #���  6  $y�Hidden=�6T  5      5      5      5      5       5!      5"      5#      