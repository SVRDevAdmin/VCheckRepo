   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null]3clr-namespace:VCheckViewer.Views.Pages.Setting.User%VCheckViewer.Views.Pages.Setting.User  5      8    2VCheckViewer.Views.Pages.Setting.User.ViewUserPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Elocal3clr-namespace:VCheckViewer.Views.Pages.Setting.User     5        N� Title$  ViewUserPage��5      .��5
      � $��	0,10,0,10q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��2��6!   $��45��65    �� CornerRadius$ 7��6A   $��1q�6R   
���   �
���   �$��	90,0,70,0q�6�   $	��500��6�   ._�5      �  5      �� 5      $
��auto �6'   $	��100��64   �� 5      $��* �6'   �� 5      $
��auto �6'   $	��100��64   �� 5      $
��auto �6'   $	��100��64   .+�5      �� $��0��6   $��2q�6)   ._�5      �� 
�������$��Stretch=�6      Themes_ButtonColor#���   6�   $��0q�6�     RoundButton#���  6�   ��6   ��  �� Path$ Setting_Label_User�  	Resources �� Source# [ 6  5       �� 5!      $��1��6   $��2q�6)   ._�5"      �� -   5        
�������$��Stretch=�5"   �   $��0q�6�   
���   �#���  6�   ��6   �� $# Setting_Label_LanguageCountry�# [ 6  5#      �� 5$      $��2��6   $��2q�6)   ._�5%      �� -   5        $��btnSettings��5%      $��PMS/LIS/HIS��60   
�������$��Stretch=�6Y   $��0q�6w   
���   �#���  6�   6�   5&      �� 5'      $��3��6   $��2q�6)   ._�5(      �� -   5         ��Name$ btnDeviceSetting��5(      
�������$��Stretch=�6�   $��0q�6�   
���   �#���  6�   ��67   �� $ Setting_Label_Device�# [ 6   5)      5*      5+      �� 5,   
   -   5        $ UserPage��5,      $��1��6#     
BackButton#��[ 60     Themes_FontColor#���  6T   6�   �� 5-   
   $��1��6   $��2��6!   ._�5.      �� $��30q�6   $��1q�6!    " Themes_UserInfoFrameBackground#���  65    �� Padding$ 20q�6t   $ 7��6�   #���  6�   ._�5/      �  50      �� 51      $
��auto �6(   �� 52      $
��auto �6(   �� 53      $
��auto �6(   �� 54      $
��auto �6(   �� 55      $
��auto �6(   �� 56      $
��auto �6(   �� 57      $
��auto �6(   �� 58      $
��auto �6(   �� 59      $
��auto �6(   �� 5:      $
��auto �6(   .+�5<      �� #���  6m   $��0��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_Title�# [ 6�   �� 5=      -   5         ��Name$ Title��5=       $��1��6/   $��
10,0,10,10q�6<   $��Center=�6P   #���  6k   �� 5>      #���  6p   $��2��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_FullName�# [ 6�   �� 5?      -   5        $ FullName��5?       $��3��62   $��
10,0,10,10q�6?   $��Center=�6S   #���  6n   �� 5@      #���  6o   $��4��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_LoginID�# [ 6�   �� 5A      -   5        $ LoginID��5A       $��5��61   $��
10,0,10,10q�6>   $��Center=�6R   #���  6m   �� 5B      #���  6o   $��6��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_StaffID�# [ 6�   �� 5C      -   5        $ StaffID��5C       $��7��61   $��
10,0,10,10q�6>   $��Center=�6R   #���  6m   �� 5D      #���  6v   $��8��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $" Setting_Label_RegistrationNo�# [ 6�   �� 5E      -	   5        $ RegistrationNo��5E       $��9��68   $��
10,0,10,10q�6E   $��Center=�6Y   #���  6t   5F      5G      5H      �� 5I   
   $��2��6   $��2��6!   ._�5J      �� $��30q�6   $��1q�6!   #���  65   $ 20q�6t   $ 7��6�   #���  6�   ._�5K      �  5L      �� 5M      $
��auto �6(   �� 5N      $
��auto �6(   �� 5O      $
��auto �6(   �� 5P      $
��auto �6(   �� 5Q      $
��auto �6(   �� 5R      $
��auto �6(   �� 5S      $
��auto �6(   �� 5T      $
��auto �6(   �� 5U      $
��auto �6(   �� 5V      $
��auto �6(   .+�5X      �� #���  6n   $��0��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_Gender�# [ 6�   �� 5Y      -
   5        $ Gender��5Y       $��1��60   $��
10,0,10,10q�6=   $��Center=�6Q   #���  6l   �� 5Z      #���  6s   $��2��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_DateOfBirth�# [ 6�   �� 5[      -   5        $ DateOfBirth��5[       $��3��65   $��
10,0,10,10q�6B   $��Center=�6V   #���  6q   �� 5\      #���  6l   $��4��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_Role�# [ 6�   �� 5]      -   5        $
 Role��5]       $��5��6.   $��
10,0,10,10q�6;   $��Center=�6O   #���  6j   �� 5^      #���  6m   $��6��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_Email�# [ 6�   �� 5_      -   5        $ EmailAddress��5_       $��7��66   $��
10,0,10,10q�6C   $��Center=�6W   #���  6r   �� 5`      #���  6n   $��8��6�   $��Bottom=�6�   $��10q�6�   $
��Bold"�6�   ��6    �� $ Setting_Label_Status�# [ 6�   �� 5a      -   5        $ Status��5a       $��9��60   $��
10,0,10,10q�6=   $��Center=�6Q   #���  6l   5b      5c      5d      �� 5e   
   -   5        $
 Edit��5e      $��Bottom=�6n   $��Stretch=�6�   $��40��6�   $��3��6�   $��2��6�   $��30q�6�   #���   6�   
�������#���  6C  #���   6i  $y�Hidden=�6�  ��6   �� $ General_Label_Edit�# [ 6�  5f      5g      