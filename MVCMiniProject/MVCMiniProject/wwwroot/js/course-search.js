(function () {
    var debounceMs = 300;
    var searchUrl = "/api/courses/search";
    var detailUrl = "/Course/Detail/";
    var emptyMessage = "Bu isimde kurs bulunamadı.";

    function debounce(fn, wait) {
        var timer;
        return function () {
            var context = this;
            var args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                fn.apply(context, args);
            }, wait);
        };
    }

    function escapeHtml(value) {
        return String(value == null ? "" : value)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;");
    }

    function imageSrc(name) {
        if (!name) {
            return "/images/courses.jpg";
        }
        return "/images/" + encodeURIComponent(name);
    }

    function bindForm(form) {
        var input = form.querySelector(".search_input");
        if (!input) {
            return;
        }

        var dropdown = form.querySelector(".course_search_dropdown");
        if (!dropdown) {
            dropdown = document.createElement("div");
            dropdown.className = "course_search_dropdown";
            dropdown.hidden = true;
            form.appendChild(dropdown);
        }

        var activeIndex = -1;
        var controller = null;
        var lastQuery = "";

        function hide() {
            dropdown.hidden = true;
            dropdown.innerHTML = "";
            activeIndex = -1;
        }

        function items() {
            return Array.prototype.slice.call(dropdown.querySelectorAll(".course_search_item"));
        }

        function highlight(index) {
            var rows = items();
            if (!rows.length) {
                activeIndex = -1;
                return;
            }
            if (index < 0) {
                index = rows.length - 1;
            }
            if (index >= rows.length) {
                index = 0;
            }
            activeIndex = index;
            rows.forEach(function (row, i) {
                row.classList.toggle("is-active", i === activeIndex);
            });
            rows[activeIndex].scrollIntoView({ block: "nearest" });
        }

        function renderLoading() {
            dropdown.hidden = false;
            dropdown.innerHTML = '<div class="course_search_status">Loading...</div>';
            activeIndex = -1;
        }

        function renderEmpty() {
            dropdown.hidden = false;
            dropdown.innerHTML = '<div class="course_search_empty">' + emptyMessage + "</div>";
            activeIndex = -1;
        }

        function renderResults(list) {
            dropdown.hidden = false;
            dropdown.innerHTML = list.map(function (course) {
                return (
                    '<a class="course_search_item" href="' + detailUrl + course.id + '">' +
                        '<img class="course_search_thumb" src="' + imageSrc(course.image) + '" alt="">' +
                        '<div class="course_search_meta">' +
                            '<div class="course_search_title">' + escapeHtml(course.title) + "</div>" +
                            '<div class="course_search_price">$' + escapeHtml(course.price) + "</div>" +
                            '<div class="course_search_excerpt">' + escapeHtml(course.excerpt) + "</div>" +
                        "</div>" +
                    "</a>"
                );
            }).join("");
            activeIndex = -1;
        }

        function searchNow(value) {
            var query = (value || "").trim();
            lastQuery = query;
            if (!query) {
                hide();
                return;
            }

            if (controller) {
                controller.abort();
            }
            controller = new AbortController();
            renderLoading();

            fetch(searchUrl + "?q=" + encodeURIComponent(query), {
                signal: controller.signal,
                headers: { Accept: "application/json" }
            })
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error("search failed");
                    }
                    return response.json();
                })
                .then(function (data) {
                    if (lastQuery !== query) {
                        return;
                    }
                    if (!data || !data.length) {
                        renderEmpty();
                        return;
                    }
                    renderResults(data);
                })
                .catch(function (error) {
                    if (error && error.name === "AbortError") {
                        return;
                    }
                    if (lastQuery === query) {
                        renderEmpty();
                    }
                });
        }

        var runSearch = debounce(searchNow, debounceMs);

        input.addEventListener("input", function () {
            runSearch(input.value);
        });

        function goOrSearch(event) {
            if (event) {
                event.preventDefault();
            }
            var rows = items();
            var selected = rows[activeIndex] || null;
            if (selected) {
                window.location.href = selected.getAttribute("href");
                return;
            }
            var query = (input.value || "").trim();
            if (!query) {
                return;
            }
            window.location.href = "/Course/Search?q=" + encodeURIComponent(query);
        }

        input.addEventListener("keydown", function (event) {
            var rows = items();
            if (event.key === "Escape") {
                hide();
                input.blur();
                return;
            }
            if (event.key === "ArrowDown") {
                if (!dropdown.hidden && rows.length) {
                    event.preventDefault();
                    highlight(activeIndex + 1);
                }
                return;
            }
            if (event.key === "ArrowUp") {
                if (!dropdown.hidden && rows.length) {
                    event.preventDefault();
                    highlight(activeIndex - 1);
                }
                return;
            }
            if (event.key === "Enter") {
                goOrSearch(event);
            }
        });

        form.addEventListener("submit", function (event) {
            goOrSearch(event);
        });

        document.addEventListener("click", function (event) {
            if (!form.contains(event.target)) {
                hide();
            }
        });
    }

    document.querySelectorAll(".header_search_form").forEach(bindForm);
})();
