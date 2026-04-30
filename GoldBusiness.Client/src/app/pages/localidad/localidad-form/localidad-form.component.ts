import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalidadService, LocalidadDTO, TipoLocalidad } from '../../../services/localidad.service';
import { EstablecimientoDTO, EstablecimientoService } from '../../../services/establecimiento.service';
import { CuentaDTO, CuentaService } from '../../../services/cuenta.service';

@Component({
  selector: 'app-localidad-form',
  templateUrl: './localidad-form.component.html',
  styleUrls: ['./localidad-form.component.css']
})
export class LocalidadFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;
  
  establecimientos: EstablecimientoDTO[] = [];
  cuentas: CuentaDTO[] = [];
  tiposLocalidad: { value: TipoLocalidad; label: string }[] = [];

  constructor(
    private fb: FormBuilder,
    private localidadService: LocalidadService,
    private establecimientoService: EstablecimientoService,
    private cuentaService: CuentaService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      establecimientoId: ['', Validators.required],
      codigo: ['', [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      
      // ══ Tipo y Capacidades Operativas (Estándares ERP) ══
      tipo: [TipoLocalidad.Almacen, Validators.required],
      permiteVentas: [false],
      permiteCompras: [false],
      permiteTransferencias: [true],
      permiteAjustes: [true],
      requiereControlLotes: [false],
      requiereNumerosSerie: [false],
      
      // ══ Cuentas Contables (GL Accounts) ══
      cuentaInventarioId: ['', Validators.required],
      cuentaCostoId: ['', Validators.required],
      cuentaVentaId: ['', Validators.required],
      cuentaDevolucionId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.tiposLocalidad = this.localidadService.getTiposLocalidad();
    this.loadEstablecimientos();
    this.loadCuentas();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
    
    // Actualizar capacidades según tipo seleccionado
    this.itemForm.get('tipo')?.valueChanges.subscribe((tipo: TipoLocalidad) => {
      this.updateCapacidadesPorTipo(tipo);
    });
  }

  loadEstablecimientos(): void {
    this.establecimientoService.getAll().subscribe({
      next: (establecimientos) => {
        this.establecimientos = establecimientos;
      },
      error: (error) => {
        console.error('Error loading establecimientos:', error);
        this.error = 'Error al cargar los establecimientos';
      }
    });
  }

  loadCuentas(): void {
    this.cuentaService.getAll().subscribe({
      next: (cuentas) => {
        this.cuentas = cuentas;
      },
      error: (error) => {
        console.error('Error loading cuentas:', error);
        this.error = 'Error al cargar las cuentas contables';
      }
    });
  }

  /**
   * Actualiza las capacidades operativas según el tipo de localidad
   */
  updateCapacidadesPorTipo(tipo: TipoLocalidad): void {
    switch (tipo) {
      case TipoLocalidad.Almacen:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: true,
          permiteTransferencias: true,
          permiteAjustes: true
        });
        break;
      case TipoLocalidad.PuntoVenta:
        this.itemForm.patchValue({
          permiteVentas: true,
          permiteCompras: false,
          permiteTransferencias: true,
          permiteAjustes: true
        });
        break;
      case TipoLocalidad.AreaRecepcion:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: true,
          permiteTransferencias: true,
          permiteAjustes: false
        });
        break;
      case TipoLocalidad.AreaDespacho:
        this.itemForm.patchValue({
          permiteVentas: true,
          permiteCompras: false,
          permiteTransferencias: true,
          permiteAjustes: false
        });
        break;
      case TipoLocalidad.Produccion:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: true,
          permiteTransferencias: true,
          permiteAjustes: true
        });
        break;
      case TipoLocalidad.Transito:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: false,
          permiteTransferencias: true,
          permiteAjustes: false
        });
        break;
      case TipoLocalidad.Cuarentena:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: false,
          permiteTransferencias: false,
          permiteAjustes: true
        });
        break;
      case TipoLocalidad.Consignacion:
        this.itemForm.patchValue({
          permiteVentas: true,
          permiteCompras: false,
          permiteTransferencias: true,
          permiteAjustes: false
        });
        break;
      case TipoLocalidad.Devoluciones:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: false,
          permiteTransferencias: true,
          permiteAjustes: true
        });
        break;
      case TipoLocalidad.Obsoletos:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: false,
          permiteTransferencias: false,
          permiteAjustes: true
        });
        break;
      default:
        this.itemForm.patchValue({
          permiteVentas: false,
          permiteCompras: false,
          permiteTransferencias: true,
          permiteAjustes: true
        });
    }
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.error = null;

    this.localidadService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading localidad:', error);
        this.error = error.message || 'Error al cargar la localidad';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = null;

    const formData: LocalidadDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.localidadService.update(this.itemId!, formData)
      : this.localidadService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/organizacion/localidad']);
      },
      error: (error) => {
        console.error('Error saving localidad:', error);
        this.error = error.message || 'Error al guardar la localidad';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/organizacion/localidad']);
  }
}
