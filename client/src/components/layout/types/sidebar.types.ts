export interface SidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

export interface NavItem {
  label: string;
  icon: string;
  active?: boolean;
  comingSoon?: boolean;
}
