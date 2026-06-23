function toggleMessage(element, message) {
  if (!element) {
    return;
  }

  if (!message) {
    element.hidden = true;
    element.textContent = "";
    return;
  }

  element.hidden = false;
  element.textContent = message;
}

async function incrementCounter(form) {
  const counterId = form.dataset.counterId;
  const counterName = form.dataset.counterName || counterId;
  const valueElement = form.closest(".state-demo__card")?.querySelector("[data-counter-value]");
  const button = form.querySelector("[data-counter-button]");
  const statusElement = document.getElementById("state-demo-status");
  const errorElement = document.getElementById("state-demo-error");

  if (!counterId || !valueElement || !button) {
    return;
  }

  button.disabled = true;
  toggleMessage(errorElement, "");

  try {
    const response = await fetch(`/api/counters/${encodeURIComponent(counterId)}/increment`, {
      method: "POST",
      headers: {
        Accept: "application/json"
      },
      credentials: "same-origin"
    });

    const payload = await response.json().catch(() => null);

    if (!response.ok || !payload) {
      throw new Error(payload?.error || "Could not increment the counter right now.");
    }

    valueElement.textContent = String(payload.value);
    toggleMessage(statusElement, `${counterName} incremented to ${payload.value}.`);
  } catch (error) {
    toggleMessage(statusElement, "");
    toggleMessage(errorElement, error instanceof Error ? error.message : "Could not increment the counter right now.");
  } finally {
    button.disabled = false;
  }
}

document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("[data-counter-form]").forEach((form) => {
    form.addEventListener("submit", async (event) => {
      event.preventDefault();
      await incrementCounter(form);
    });
  });
});
