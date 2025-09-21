// ITLA Networking - JavaScript personalizado

document.addEventListener("DOMContentLoaded", () => {
  // Import Bootstrap
  const bootstrap = window.bootstrap

  // Inicializar tooltips de Bootstrap
  var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
  var tooltipList = tooltipTriggerList.map((tooltipTriggerEl) => new bootstrap.Tooltip(tooltipTriggerEl))

  // Inicializar popovers de Bootstrap
  var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
  var popoverList = popoverTriggerList.map((popoverTriggerEl) => new bootstrap.Popover(popoverTriggerEl))

  // Auto-resize para textareas
  const textareas = document.querySelectorAll("textarea")
  textareas.forEach((textarea) => {
    textarea.addEventListener("input", function () {
      this.style.height = "auto"
      this.style.height = this.scrollHeight + "px"
    })
  })

  // Confirmación para acciones destructivas
  const deleteButtons = document.querySelectorAll("[data-confirm]")
  deleteButtons.forEach((button) => {
    button.addEventListener("click", function (e) {
      const message = this.getAttribute("data-confirm") || "¿Estás seguro?"
      if (!confirm(message)) {
        e.preventDefault()
      }
    })
  })

  // Previsualización de imágenes
  const imageInputs = document.querySelectorAll('input[type="file"][accept*="image"]')
  imageInputs.forEach((input) => {
    input.addEventListener("change", (e) => {
      const file = e.target.files[0]
      if (file) {
        const reader = new FileReader()
        reader.onload = (e) => {
          const preview = document.getElementById(input.id + "-preview")
          if (preview) {
            preview.src = e.target.result
            preview.style.display = "block"
          }
        }
        reader.readAsDataURL(file)
      }
    })
  })

  // Contador de caracteres para textareas
  const textareasWithCounter = document.querySelectorAll("textarea[data-max-length]")
  textareasWithCounter.forEach((textarea) => {
    const maxLength = Number.parseInt(textarea.getAttribute("data-max-length"))
    const counter = document.createElement("small")
    counter.className = "text-muted"
    textarea.parentNode.appendChild(counter)

    function updateCounter() {
      const remaining = maxLength - textarea.value.length
      counter.textContent = `${remaining} caracteres restantes`
      counter.className = remaining < 0 ? "text-danger" : "text-muted"
    }

    textarea.addEventListener("input", updateCounter)
    updateCounter()
  })

  // Búsqueda en tiempo real
  const searchInputs = document.querySelectorAll("[data-search-target]")
  searchInputs.forEach((input) => {
    const targetSelector = input.getAttribute("data-search-target")
    const targets = document.querySelectorAll(targetSelector)

    input.addEventListener("input", function () {
      const searchTerm = this.value.toLowerCase()
      targets.forEach((target) => {
        const text = target.textContent.toLowerCase()
        target.style.display = text.includes(searchTerm) ? "" : "none"
      })
    })
  })

  // Notificaciones toast
  window.showToast = (message, type = "info") => {
    const toastContainer = document.getElementById("toast-container") || createToastContainer()
    const toast = createToast(message, type)
    toastContainer.appendChild(toast)

    const bsToast = new bootstrap.Toast(toast)
    bsToast.show()

    toast.addEventListener("hidden.bs.toast", () => {
      toast.remove()
    })
  }

  function createToastContainer() {
    const container = document.createElement("div")
    container.id = "toast-container"
    container.className = "toast-container position-fixed top-0 end-0 p-3"
    container.style.zIndex = "1055"
    document.body.appendChild(container)
    return container
  }

  function createToast(message, type) {
    const toast = document.createElement("div")
    toast.className = "toast"
    toast.setAttribute("role", "alert")
    toast.innerHTML = `
            <div class="toast-header">
                <i class="fas fa-${getIconForType(type)} me-2 text-${type}"></i>
                <strong class="me-auto">ITLA Networking</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        `
    return toast
  }

  function getIconForType(type) {
    const icons = {
      success: "check-circle",
      error: "exclamation-triangle",
      warning: "exclamation-triangle",
      info: "info-circle",
    }
    return icons[type] || "info-circle"
  }

  // Lazy loading para imágenes
  const images = document.querySelectorAll("img[data-src]")
  const imageObserver = new IntersectionObserver((entries, observer) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        const img = entry.target
        img.src = img.dataset.src
        img.removeAttribute("data-src")
        observer.unobserve(img)
      }
    })
  })

  images.forEach((img) => imageObserver.observe(img))

  // Smooth scroll para enlaces internos
  const internalLinks = document.querySelectorAll('a[href^="#"]')
  internalLinks.forEach((link) => {
    link.addEventListener("click", function (e) {
      const targetId = this.getAttribute("href").substring(1)
      const targetElement = document.getElementById(targetId)
      if (targetElement) {
        e.preventDefault()
        targetElement.scrollIntoView({
          behavior: "smooth",
          block: "start",
        })
      }
    })
  })

  // Validación de formularios en tiempo real
  const forms = document.querySelectorAll("form[data-validate]")
  forms.forEach((form) => {
    const inputs = form.querySelectorAll("input, textarea, select")
    inputs.forEach((input) => {
      input.addEventListener("blur", function () {
        validateField(this)
      })
    })
  })

  function validateField(field) {
    const value = field.value.trim()
    const type = field.type
    const required = field.hasAttribute("required")
    let isValid = true
    let message = ""

    if (required && !value) {
      isValid = false
      message = "Este campo es requerido"
    } else if (value) {
      switch (type) {
        case "email":
          const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
          if (!emailRegex.test(value)) {
            isValid = false
            message = "Ingresa un email válido"
          }
          break
        case "tel":
          const phoneRegex = /^[\d\s\-$$$$+]+$/
          if (!phoneRegex.test(value)) {
            isValid = false
            message = "Ingresa un teléfono válido"
          }
          break
        case "password":
          if (value.length < 6) {
            isValid = false
            message = "La contraseña debe tener al menos 6 caracteres"
          }
          break
      }
    }

    updateFieldValidation(field, isValid, message)
  }

  function updateFieldValidation(field, isValid, message) {
    field.classList.remove("is-valid", "is-invalid")
    field.classList.add(isValid ? "is-valid" : "is-invalid")

    let feedback = field.parentNode.querySelector(".invalid-feedback, .valid-feedback")
    if (!feedback) {
      feedback = document.createElement("div")
      field.parentNode.appendChild(feedback)
    }

    feedback.className = isValid ? "valid-feedback" : "invalid-feedback"
    feedback.textContent = message
  }

  // Auto-save para formularios largos
  const autoSaveForms = document.querySelectorAll("form[data-auto-save]")
  autoSaveForms.forEach((form) => {
    const formId = form.id || "form-" + Date.now()
    const inputs = form.querySelectorAll("input, textarea, select")

    // Cargar datos guardados
    loadFormData(formId, inputs)

    // Guardar cambios
    inputs.forEach((input) => {
      input.addEventListener(
        "input",
        debounce(() => {
          saveFormData(formId, inputs)
        }, 1000),
      )
    })
  })

  function saveFormData(formId, inputs) {
    const data = {}
    inputs.forEach((input) => {
      if (input.type !== "password") {
        data[input.name] = input.value
      }
    })
    localStorage.setItem("form-" + formId, JSON.stringify(data))
  }

  function loadFormData(formId, inputs) {
    const savedData = localStorage.getItem("form-" + formId)
    if (savedData) {
      const data = JSON.parse(savedData)
      inputs.forEach((input) => {
        if (data[input.name] && input.type !== "password") {
          input.value = data[input.name]
        }
      })
    }
  }

  function debounce(func, wait) {
    let timeout
    return function executedFunction(...args) {
      const later = () => {
        clearTimeout(timeout)
        func(...args)
      }
      clearTimeout(timeout)
      timeout = setTimeout(later, wait)
    }
  }
})

// Funciones globales
window.ITLANetworking = {
  // Confirmar acción
  confirm: (message, callback) => {
    if (confirm(message)) {
      callback()
    }
  },

  // Mostrar loading
  showLoading: (element) => {
    const originalText = element.textContent
    element.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Cargando...'
    element.disabled = true
    return originalText
  },

  // Ocultar loading
  hideLoading: (element, originalText) => {
    element.innerHTML = originalText
    element.disabled = false
  },

  // Formatear fecha
  formatDate: (date) =>
    new Date(date).toLocaleDateString("es-DO", {
      year: "numeric",
      month: "long",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    }),

  // Copiar al portapapeles
  copyToClipboard: (text) => {
    navigator.clipboard.writeText(text).then(() => {
      window.showToast("Copiado al portapapeles", "success")
    })
  },
}
