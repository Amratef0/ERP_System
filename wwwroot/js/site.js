// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ---------- Submenu Toggle (Desktop) ----------
function subMenu() {
    const menuToggle = document.getElementById('menuToggle');
    const submenu = document.getElementById('submenu');
    const app = document.getElementById('app');

    if (!menuToggle || !submenu || !app) return;

    menuToggle.addEventListener('click', () => {
        const nowHidden = submenu.classList.toggle('hidden');
        app.classList.toggle('submenu-hidden', nowHidden);

        submenu.setAttribute('aria-expanded', String(!nowHidden));
        menuToggle.setAttribute('aria-pressed', String(nowHidden));
    });
}
subMenu();



function ActiveSidebar() {
    const sidebarIcons = document.querySelectorAll(".sidebar-icons button[id^='icon']");
    const submenuSections = document.querySelectorAll(".submenu-section");
    const navLinks = document.querySelectorAll(".nav-link");

    let lastActiveSection = localStorage.getItem("activeSection") || "Home";

    function activateSection(sectionId) {

        submenuSections.forEach(sec => sec.classList.add("hidden"));

        sidebarIcons.forEach(btn => btn.classList.remove("active"));

        const targetSection = document.getElementById(sectionId);
        const targetIcon = document.getElementById(`icon${sectionId}`);

        if (targetSection) targetSection.classList.remove("hidden");
        if (targetIcon) targetIcon.classList.add("active");

        localStorage.setItem("activeSection", sectionId);
    }

    activateSection(lastActiveSection);

    sidebarIcons.forEach(icon => {
        icon.addEventListener("click", () => {
            const sectionId = icon.id.replace("icon", "");
            activateSection(sectionId);
        });
    });

    navLinks.forEach(link => {
        link.addEventListener("click", function () {
            navLinks.forEach(l => l.classList.remove("active"));
            this.classList.add("active");
            localStorage.setItem("activeLink", this.href);
        });
    });

    const lastActiveLink = localStorage.getItem("activeLink");
    if (lastActiveLink) {
        const matchingLink = [...navLinks].find(l => l.href === lastActiveLink);
        if (matchingLink) matchingLink.classList.add("active");
    }
}

document.addEventListener("DOMContentLoaded", ActiveSidebar);




// ---------- Dark Mode Toggle ----------
function darkMood() {
    document.addEventListener("DOMContentLoaded", () => {
        const html = document.documentElement;
        const btn = document.getElementById("toggle-theme");

        if (localStorage.getItem("theme") === "dark") {
            html.classList.add("dark");
            btn.querySelector("i").classList.replace("fa-moon", "fa-sun");
        }

        btn.addEventListener("click", () => {
            html.classList.toggle("dark");
            const isDark = html.classList.contains("dark");

            localStorage.setItem("theme", isDark ? "dark" : "light");
            btn.querySelector("i").classList.replace(
                isDark ? "fa-moon" : "fa-sun",
                isDark ? "fa-sun" : "fa-moon"
            );
        });
    });
}
darkMood();


document.addEventListener('DOMContentLoaded', function () {
    const menuToggleMobile = document.getElementById('menuToggleMobile');
    const overlay = document.getElementById('overlay');
    const sidebarIcons = document.querySelector('.sidebar-icons');

    if (!menuToggleMobile || !overlay || !sidebarIcons) return;

    function toggleSidebar() {
        document.body.classList.toggle('mobile-sidebar-open');
    }

    function closeSidebar(e) {
        const sidebarOpen = document.body.classList.contains('mobile-sidebar-open');
        if (
            sidebarOpen &&
            !e.target.closest('.sidebar-icons') &&
            !e.target.closest('#menuToggleMobile')
        ) {
            document.body.classList.remove('mobile-sidebar-open');
        }
    }

    menuToggleMobile.addEventListener('click', toggleSidebar);
    overlay.addEventListener('click', toggleSidebar);
    document.addEventListener('click', closeSidebar);
});
