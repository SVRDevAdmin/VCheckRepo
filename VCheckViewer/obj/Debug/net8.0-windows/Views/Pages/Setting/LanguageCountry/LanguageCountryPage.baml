   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nulls>clr-namespace:VCheckViewer.Views.Pages.Setting.LanguageCountry0VCheckViewer.Views.Pages.Setting.LanguageCountry  5      J    DVCheckViewer.Views.Pages.Setting.LanguageCountry.LanguageCountryPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Plocal>clr-namespace:VCheckViewer.Views.Pages.Setting.LanguageCountry     5      A =Wpf.Ui, Version=3.0.5.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  6T     N� Title$  LanguageCountryPage��5       � 	Resources 5
         TextBoxStyle �� 
TargetType �� Property   Themes_SearchFieldBackground �� Value  Themes_SearchFieldFontColor��%�   &	        �� 5   
   " ��6%   .�5      ��  � ��# �  6+   �� 5       � ��$ Center=�69   �� 5       � ��# �  6+   �� 5       � ��$ 
5, 2, 0, 0q�6(   5      5      .��5      � $��	0,10,0,10q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��2��6!   $��45��65    �� CornerRadius$ 7��6A   $��1q�6R   
���   �
���   �$��	90,0,70,0q�6�   $	��500��6�   ._�5       �  5!      �� 5"      $
��auto �6'   $	��100��64   �� 5#      $��* �6'   �� 5$      $
��auto �6'   $	��100��64   �� 5%      $
��auto �6'   $	��100��64   .+�5'      �� $��0��6   $��2q�6)   ._�5(      �� -   5        $��btnUserSetting��5(      
�������$��Stretch=�6�   $��0q�6�   
���   �  RoundButton#���  6�   ��63   ��  �� Path$ Setting_Label_User�  	Resources	 �� Source#	 [ 6  5)      �� 5*      $��1��6   $��2q�6)   ._�5+      �� 
�������$��Stretch=�6�   $��0q�6�     Themes_ButtonColor#���  6�   #���  6�   ��6   �� $# Setting_Label_LanguageCountry�#	 [ 6  5,      �� 5-      $��2��6   $��2q�6)   ._�5.      �� -   5        $��btnSettings��5.      $��PMS/LIS/HIS��60   
�������$��Stretch=�6Y   $��0q�6w   
���   �#���  6�   6�   5/      �� 50      $��3��6   $��2q�6)   ._�51      �� -   5        $��btnDeviceSetting��51      
�������$��Stretch=�6�   $��0q�6�   
���   �#���  6�   ��65   �� $ Setting_Label_Device�#	 [ 6  52      53      54      �� 56   
   $��4��6   $��1��6%   ._�57      �� -   5        $��	ErrorText��57      $��Center=�6)   
���  ��$��10q�6W   $y�Hidden=�6c   58      �� 59   
   $��1��6   $��1��6!   ._�5:      �  5;      �� 5<      $
��auto �6$   �� 5=      $��* �6$   .+�5?      �� $��0��6   $��30,30,30,10q�6&   $��1q�6;    % !Themes_CountryLangFrameBackground#���  6O   $ 7��6�   $��40��6�   #���  6�   ._�5@      � $��10q�6    5A      �� 5B      $��* �6/   �� 5C      $��2* �6/   .+�5E      ��   Themes_FontColor#���  6t   $��0��6�   $��Center=�6�   $
��Bold"�6�   ��6$   �� $ Setting_Label_Language�#	 [ 6�   �� 5F      -   5        
 ��Name$
 SelectedLanguage��5F   $   $��1��6�   $��Center=�6�   
��� ���$
��Bold"�6�   ��6>   �� $" Setting_Label_SelectLanguage�#	 [ 6�   5G      5H      �� 5I      $��1��6   $��
30,0,30,30q�6&   $��1q�6:   #���  6N    �� Padding$ 10q�6�   $ 7��6�   #���  6�   ._�5J      �� -   5        $��LanguageListView��5J      #���  67   $��Stretch=�6y   $��0,0,20,0q�6�   #���  6�   5K      5L      5M      5N      �� 5O   
   $��2��6   $��1��6!   ._�5P      �  5Q      �� 5R      $
��auto �6$   �� 5S      $��* �6$   .+�5U      �� $��0��6   $��30,30,30,10q�6&   $��1q�6;   #���  6O   $ 7��6�   $��40��6�   #���  6�   ._�5V      � $��10q�6    5W      �� 5X      $��* �6/   �� 5Y      $��2* �6/   .+�5[      �� #���  6s   $��0��6�   $��Center=�6�   $
��Bold"�6�   ��6$   �� $ Setting_Label_Country�#	 [ 6�   �� 5\      -   5        $
 SelectedCountry��5\   $   $��1��6�   $��Center=�6�   $
��Bold"�6�   
��� �����6=   �� $! Setting_Label_SelectCountry�#	 [ 6�   5]      5^      �� 5_      $��1��6   $��
30,0,30,30q�6&   $��1q�6:   #���  6N   $ 10q�6�   $ 7��6�   #���  6�   ._�5`      �  5a      �� 5b      $
��auto �6,   �� 5c      $	��460 �6,   .+�5e      �� $��Stretch=�6!   $	��Top=�6?   $��2��6W   $��0��6g   $��	10,0,10,0q�6t   ._�5f      ��  & "Themes_CountryLangSearchBackground#���  6%   $ 7��6g   $ 5q�6x   $��1q�6�   #���  6�   ._�5g   "   � .+�5i   &   ��  	 Themes_SearchIcon#��� 	 6,   $��24��6Y   $
��Left=�6d   $��
5, 2, 0, 0q�6   �� 5k   &   -   5         ��Name$ CountrySearchBar��5k   .   $��0q�6H   $��	30,0,10,0q�6\   $��Center=�6o   #��[  6�   �� 5l   &    J� IsHitTestVisible$ False��60   $��Center=�6�   $
��Left=�6�   $��	40,0,10,0q�6�   
���������6I   �� $ General_Label_Searchbar�#	 [ ��5m   *   �� 5n   .   " ��64   .�5o   2   ��  � y�$ 	Collapsed=�6O    �� Triggers 5p   2   �� 5q   6    �� Binding 6B   �� *Text+ �� ElementName$ CountrySearchBar�� �� Value  6y   .a�5r   :   ��  � y�$ Visible=�6W   5s   7   5u   /   5w   '   5x   #   5y      5z      �� 5{      $��1��6!   ._�5|      �� -	   5        $��CountryListView��5|   '   
������ $��0q�6W   $��0,10,0,0q�6k   $��Stretch=�6}   $��0,0,20,0q�6�   5}      5~      5      5�      5�      5�      �� 5�   
   -
   5         ��Name$ 
SaveButton��5�      $��Bottom=�5�      $��Stretch=�6,   $��40��6J   $��3��6V   $��2��6f   $��30q�6s   #���  5�       
 Themes_ButtonFontColor#��� 
 6C   #���  5�      #���  67   ��5�   %   �� $ General_Label_Save�#	 [ 5�      5�      5�      