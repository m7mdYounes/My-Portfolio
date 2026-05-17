(function () {
    const navbar = document.querySelector(".portfolio-navbar");

    function handleNavbarScroll() {
        if (!navbar) {
            return;
        }

        if (window.scrollY > 40) {
            navbar.classList.add("navbar-scrolled");
        } else {
            navbar.classList.remove("navbar-scrolled");
        }
    }

    window.addEventListener("scroll", handleNavbarScroll);
    handleNavbarScroll();
})();