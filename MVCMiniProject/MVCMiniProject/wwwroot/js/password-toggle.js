(function () {
    var eye = '<svg viewBox="0 0 24 24" aria-hidden="true"><path d="M1 12s4-7 11-7 11 7 11 7-4 7-11 7-11-7-11-7z"></path><circle cx="12" cy="12" r="3"></circle></svg>';
    var eyeOff = '<svg viewBox="0 0 24 24" aria-hidden="true"><path d="M17.94 17.94A10.94 10.94 0 0 1 12 19c-7 0-11-7-11-7a21.8 21.8 0 0 1 5.06-5.94"></path><path d="M9.9 4.24A10.94 10.94 0 0 1 12 5c7 0 11 7 11 7a21.77 21.77 0 0 1-2.16 3.19"></path><line x1="1" y1="1" x2="23" y2="23"></line><circle cx="12" cy="12" r="3"></circle></svg>';

    function wrap(input) {
        if (input.closest(".js-password-wrap")) {
            return;
        }

        var wrapEl = document.createElement("div");
        wrapEl.className = "js-password-wrap";
        input.parentNode.insertBefore(wrapEl, input);
        wrapEl.appendChild(input);

        var button = document.createElement("button");
        button.type = "button";
        button.className = "js-password-toggle";
        button.setAttribute("aria-label", "Show password");
        button.innerHTML = eye;
        wrapEl.appendChild(button);

        button.addEventListener("click", function () {
            var hidden = input.type === "password";
            input.type = hidden ? "text" : "password";
            button.setAttribute("aria-label", hidden ? "Hide password" : "Show password");
            button.innerHTML = hidden ? eyeOff : eye;
        });
    }

    document.querySelectorAll('input[type="password"]').forEach(wrap);
})();
