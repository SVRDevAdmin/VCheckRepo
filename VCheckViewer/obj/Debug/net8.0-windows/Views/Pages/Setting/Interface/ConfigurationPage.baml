   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullg8clr-namespace:VCheckViewer.Views.Pages.Setting.Interface*VCheckViewer.Views.Pages.Setting.Interface  5      [ WSystem.Windows.Forms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5      B    <VCheckViewer.Views.Pages.Setting.Interface.ConfigurationPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jlocal8clr-namespace:VCheckViewer.Views.Pages.Setting.Interface     5      Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5        N� Title$  ConfigurationPage��5
      .��5      � $��0, 10, 0, 10q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��2��6!   $��45��65    �� CornerRadius$ 7��6A   $��1q�6R   
���   �
���   �$��	90,0,70,0q�6�   $	��500��6�   ._�5      �  5      �� 5      $
��auto �6'   $	��100��64   �� 5      $��* �6'   �� 5      $
��auto �6'   $	��100��64   �� 5      $
��auto �6'   $	��100��64   .+�5       �� $��0��6   $��2q�6)   ._�5!      �� -   5        $��btnUser��5!   l   
���   �$��0q�6�   
�������$��Stretch=�6�      RoundButton#���   6�   ��6   ��  �� Path$ Setting_Label_User�  	Resources �� Source# [ 6  5"      �� 5#      $��1��6   $��2q�6)   ._�5$      �� -   5        $��btnLanguage��5$   w   
�������$��Stretch=�6�   $��0q�6�   
���   �#���   6�   ��6   �� $# Setting_Label_LanguageCountry�# [ 6$  5%      �� 5&      $��2��6   $��2q�6)   ._�5'      �� $��PMS/LIS/HIS��6     Themes_ButtonColor#���  63   
�������$��Stretch=�6x   $��0q�6�   #���   6�   6�   5(      �� 5)      $��3��6   $��2q�6)   ._�5*      �� -   5        $��	btnDevice��5*   n   
�������$��Stretch=�6�   $��0q�6�   
���   �#���   6�   ��6   �� $ Setting_Label_Device�# [ 6  5+      5,      5-      �� 5.   
   -   5        $��borderConfiguration��5.      $��1��6,   $��2��6<   $��2��6P   
������ $��0q�6w   ._�5/      �� $��180, 65, 210, 160q�6   $��1q�60    # Themes_InterfaceFrameBackground#���  6D    �� Padding$ 10q�6�   $ 7��6�   #���  6�   ._�50      �  51      �� 52      $
��auto �6(   �� 53      $
��auto �6(   �� 54      $
��auto �6(   �� 55      $
��auto �6(   �� 56      $
��auto �6(   �� 57      $
��auto �6(   .+�5:      ��   Themes_FontColor#���  6y   $��1��6�   $��Bottom=�6�   $��	Noto Sans)�6�   $��13(�6�   $
��Bold"�6�   $��10, 30, 0, 10q�6  ��6    �� $% Configuration_Label_PMS_LIS_HIS�# [ 6!  �� 5;      -   5        $��borderIP��5;      $��2��6-   
�������$ 5, 12, 0, 0q�6M   
���   �$��1q�6w   $ 7��6�   ._�5<      7  1System.Windows.Forms.Integration.WindowsFormsHost  -   5        $��10, 0q�5<   C   $��30��6R   $��Bottom=�6^   $
��Left=�6y      
FontFamily$ 	Noto Sans)�6�      FontSize$ 12Q�6�   	   
FontWeight$	 Light"�6�   
   
Background

 �����   Child. 5=      (  "System.Windows.Forms.MaskedTextBox  -   5        	txtIP��   Mask$ ###.###.###.###��5=   >   "  System.Windows.Forms.Control   Width$	 350��6U      Height$ 30��6a      	TextAlign$
 Left=�6m   &   System.Windows.Forms.TextBoxBase   BorderStyle$
 None=�6~   5>      5?      �� 5A      #���  6�   $��3��6�   $��Bottom=�6�   $��	Noto Sans)�6�   $��13(�6�   $
��Bold"�6�   $��10, 30, 0, 10q�6  ��6    �� $, &Configuration_Label_PMS_LIS_HIS_PortNo�# [ 6(  �� 5B      -   5        $��borderPortNo��5B      $��4��61   
�������$ 5q�6Q   
���   �$��1q�6q   $ 7��6�   ._�5C      �� -	   5         ��Name$ 	txtPortNo��5C   "   $��Center=�65   $��0q�6P    �� 	MaxLength$ 3��6�   6�   5D      �� 5F      -
   5        $��BorderButton��5F      $��5��61   $��0, 30q�6>   ._�5G      �� -   5         ��Name$ 	btnUpdate��5G   !   $��Center=�6�   $��Stretch=�6�   
�������#���  5H   !   #���  6S   $��0q�6�   #���   6�   ${�False��5I   !   ��5G   4   �� $' !Configuration_Label_Button_Update�# [ 5I   3   5J      5K      5L      5M      5N      5O      