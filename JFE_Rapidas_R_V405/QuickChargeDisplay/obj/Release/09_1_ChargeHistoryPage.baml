   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  *    $QuickChargeDisplay.ChargeHistoryPage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    0local clr-namespace:QuickChargeDisplay    -     N� Title$  ChargeHistoryPage��$	��800��$	��480�� � 	Resources  �� 
TargetType   ButtonTextBlock �� Property �� Value  GridViewHeaderTextBlock��%�  &	        &	 �     �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �   ���  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .   N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.gif��.���� .+��� -    ��Name$ chargeHistoryList�� �� Left$ 81��
	 �� Top$		 131��$	��712��$	��347������ 
 �� Path$

 List�   QuickChargeDisplay.QCDV   ChargeHistory �� Source# Z  �� View �� .P��� .���� #��[ NO �� CellTemplate �� .R�� $��80��$��30��.+���  �� TextAlignment$ Right=�$��15(�$��Right=�$��Center=����� $
 Number��� .���� #��[ Date �� .R��� $ Center=�$��15(�$	��150������ $

 Date��� .���� #��[ Time �� .R��� $ Center=�$��15(�$	��150������ $

 Time��� .���� #��[ Info �� .R��� $
 Left=�$��15(�$	��450������ $
 Details��� $<��6image\10_관리자화면\아이콘\충전내역_ON.gif��$	 324��$	 22��$��50��$��48���� #��[  $��30(�$	 382��$	 28��Charge History�� -    ��Name$ logOut��$4��.image\10_관리자화면\아이콘\LOGOUT1.gif��$	 738��$	 16��$��50��$��55���� -   $ ChargeDetails��$<��6image\10_관리자화면\아이콘\충전내역_On.gif��$ 15��$	 98��$��50��$��50���� -   $ TroubleDetails��$=��7image\10_관리자화면\아이콘\고장내역_OFF.gif��$ 19��$		 197��$��50��$��50���� -   $ ManualControl��$=��7image\10_관리자화면\아이콘\수동충전_OFF.gif��$ 19��$		 298��$��50��$��50���� -   $ SetupEquipment��$=��7image\10_관리자화면\아이콘\기기설정_OFF.gif��$ 19��$		 401��$��50��$��50���� $ 5��$		 150��#��[  $��16(�$
��Wrap=�$��40��$��64��$ Center=�Charge History�� $ 6��$		 248��#��[  $��16(�$
��Wrap=�$��40��$��64��$ Center=�Fault History�� $ 6��$		 349��#��[  $��16(�$
��Wrap=�$��40��$��64��$ Center=�Manual Charge�� $ 6��$		 452��#��[  $��16(�$
��Wrap=�$��25��$��64��$ Center=�Setup�� $	 523��$	 96��$��font\YoonYGO550 TT.ttf)�$
��Bold"�$��20(�$��34.296��$��66��$
��DATE���� -    ��Name$ cbHistoryDate��$	 591��$	 98��$	��200��$��30.009��