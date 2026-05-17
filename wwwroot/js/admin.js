(function () {
    window.addResponsibilityRow = function () {
        const container = document.getElementById("responsibilities-container");

        if (!container) {
            return;
        }

        const index = container.querySelectorAll(".dynamic-list-item").length;

        const wrapper = document.createElement("div");
        wrapper.className = "dynamic-list-item";

        wrapper.innerHTML = `
            <input name="Responsibilities[${index}]" class="form-control" placeholder="Responsibility item" />
            <button type="button" class="btn btn-outline-danger" onclick="removeDynamicRow(this)">
                <i class="bi bi-trash"></i>
            </button>
        `;

        container.appendChild(wrapper);
    };

    window.removeDynamicRow = function (button) {
        const row = button.closest(".dynamic-list-item");

        if (row) {
            row.remove();
        }

        reindexResponsibilities();
    };

    function reindexResponsibilities() {
        const container = document.getElementById("responsibilities-container");

        if (!container) {
            return;
        }

        const inputs = container.querySelectorAll("input");

        inputs.forEach(function (input, index) {
            input.name = `Responsibilities[${index}]`;
        });
    }
})();