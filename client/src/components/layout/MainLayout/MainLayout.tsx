import { useState } from "react";
import { Navbar } from "@/components/layout/Navbar/Navbar";
import { Sidebar } from "@/components/layout/Sidebar/Sidebar";
import type { MainLayoutProps } from "../types/mainLayout.types";
import styles from "./MainLayout.module.css";

export function MainLayout({ children }: MainLayoutProps) {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className={styles.shell}>
      <Navbar onToggleSidebar={() => setSidebarOpen((open) => !open)} />
      <div className={styles.body}>
        <Sidebar isOpen={sidebarOpen} onClose={() => setSidebarOpen(false)} />
        <main className={styles.content}>{children}</main>
      </div>
    </div>
  );
}
