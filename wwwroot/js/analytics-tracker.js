(function () {
    function trackClick(element) {
        const payload = {
            eventType: element.getAttribute("data-track-event"),
            componentName: element.getAttribute("data-track-component"),
            targetType: element.getAttribute("data-track-target-type"),
            targetId: element.getAttribute("data-track-target-id"),
            targetText: element.getAttribute("data-track-target-text") || element.innerText,
            pagePath: window.location.pathname,
            metadataJson: element.getAttribute("data-track-metadata")
        };

        if (!payload.eventType || !payload.componentName) {
            return;
        }

        fetch("/analytics/track-click", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "same-origin",
            body: JSON.stringify(payload)
        }).catch(function () {
            // Analytics failure should never break user experience.
        });
    }

    document.addEventListener("click", function (event) {
        const element = event.target.closest("[data-track-event]");

        if (!element) {
            return;
        }

        trackClick(element);
    });
})();