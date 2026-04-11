import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProveedorService } from '../../../services/proveedor.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService } from '../../../services/provincia.service';
import { MunicipioService } from '../../../services/municipio.service';
import { CodigoPostalService } from '../../../services/codigo-postal.service';
import { TranslationService } from '../../../services/translation.service';
import { 
  TipoIdentificacionFiscal, 
  RegimenFiscal,
  TIPO_IDENTIFICACION_FISCAL_OPTIONS,
  REGIMEN_FISCAL_OPTIONS 
} from '../../../services/fiscal.types';

@Component({
  selector: 'app-proveedor-form',
  templateUrl: './proveedor-form.component.html',
  styleUrls: ['./proveedor-form.component.css']
})
export class ProveedorFormComponent implements OnInit {
  itemForm!: FormGroup; // ⭐ RENOMBRADO
  proveedorId?: number;
  isEditMode = false;
  
  error: string | null = null;
  saving = false;
  loading = false;
  
  selectedPais?: PaisDTO; // ⭐ AGREGADO

  paises: any[] = [];
  provincias: any[] = [];
  municipios: any[] = [];
  codigosPostales: any[] = [];

  tiposIdentificacionFiscal = TIPO_IDENTIFICACION_FISCAL_OPTIONS;
  regimenesFiscales = REGIMEN_FISCAL_OPTIONS;

  constructor(
    private fb: FormBuilder,
    private proveedorService: ProveedorService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private codigoPostalService: CodigoPostalService,
    private route: ActivatedRoute,
    private router: Router,
    public translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadPaises();

    this.proveedorId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.proveedorId) {
      this.isEditMode = true;
      this.loadProveedor(this.proveedorId);
    }
  }

  private initForm(): void {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(5)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      nif: ['', Validators.maxLength(11)],
      iban: ['', Validators.maxLength(27)],
      bicoSwift: ['', Validators.maxLength(11)],
      iva: [0, [Validators.required, Validators.min(-0.01), Validators.max(99.99)]],
      direccion: ['', Validators.maxLength(256)],
      telefono1: ['', Validators.maxLength(50)],
      telefono2: ['', Validators.maxLength(50)],
      paisId: [null],
      provinciaId: [{ value: null, disabled: true }],
      municipioId: [{ value: null, disabled: true }],
      codigoPostalId: [{ value: null, disabled: true }],
      web: ['', Validators.maxLength(256)],
      email1: ['', [Validators.email, Validators.maxLength(256)]],
      email2: ['', [Validators.email, Validators.maxLength(256)]],
      fax1: ['', Validators.maxLength(50)],
      fax2: ['', Validators.maxLength(50)],
      tipoIdentificadorFiscal: [null],
      regimenFiscal: [null],
      exentoIva: [false],
      extranjero: [false],
      codigoPaisIso: ['', Validators.maxLength(3)],
      validarIdentificadorFiscal: [false],
      inversionSujetoPasivo: [false]
    });

    this.setupGeographicCascade();
  }

  private setupGeographicCascade(): void {
    this.itemForm.get('paisId')?.valueChanges.subscribe((paisId: number | null) => {
      if (paisId) {
        this.loadProvincias(paisId);
        this.loadPaisDetails(paisId);
        this.itemForm.get('provinciaId')?.enable();
      } else {
        this.selectedPais = undefined;
        this.provincias = [];
        this.municipios = [];
        this.codigosPostales = [];
        this.itemForm.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null });
        this.itemForm.get('provinciaId')?.disable();
        this.itemForm.get('municipioId')?.disable();
        this.itemForm.get('codigoPostalId')?.disable();
      }
    });

    this.itemForm.get('provinciaId')?.valueChanges.subscribe((provinciaId: number | null) => {
      if (provinciaId) {
        this.loadMunicipios(provinciaId);
        this.itemForm.get('municipioId')?.enable();
      } else {
        this.municipios = [];
        this.codigosPostales = [];
        this.itemForm.patchValue({ municipioId: null, codigoPostalId: null });
        this.itemForm.get('municipioId')?.disable();
        this.itemForm.get('codigoPostalId')?.disable();
      }
    });

    this.itemForm.get('municipioId')?.valueChanges.subscribe((municipioId: number | null) => {
      if (municipioId) {
        this.loadCodigosPostales(municipioId);
        this.itemForm.get('codigoPostalId')?.enable();
      } else {
        this.codigosPostales = [];
        this.itemForm.patchValue({ codigoPostalId: null });
        this.itemForm.get('codigoPostalId')?.disable();
      }
    });
  }

  private loadPaises(): void {
    this.paisService.getAll().subscribe({
      next: (data: any) => this.paises = data,
      error: (err: any) => console.error('Error loading paises:', err)
    });
  }

  private loadPaisDetails(paisId: number): void {
    this.paisService.getById(paisId).subscribe({
      next: (pais: PaisDTO) => this.selectedPais = pais,
      error: () => this.selectedPais = undefined
    });
  }

  private loadProvincias(paisId: number): void {
    this.provinciaService.getByPaisId(paisId).subscribe({
      next: (data: any) => this.provincias = data,
      error: (err: any) => console.error('Error loading provincias:', err)
    });
  }

  private loadMunicipios(provinciaId: number): void {
    this.municipioService.getByProvinciaId(provinciaId).subscribe({
      next: (data: any) => this.municipios = data,
      error: (err: any) => console.error('Error loading municipios:', err)
    });
  }

  private loadCodigosPostales(municipioId: number): void {
    this.codigoPostalService.getByMunicipioId(municipioId).subscribe({
      next: (data: any) => this.codigosPostales = data,
      error: (err: any) => console.error('Error loading codigos postales:', err)
    });
  }

  private loadProveedor(id: number): void {
    this.loading = true;
    this.proveedorService.getById(id).subscribe({
      next: (proveedor: any) => {
        if (proveedor.paisId) {
          this.loadProvincias(proveedor.paisId);
          this.loadPaisDetails(proveedor.paisId);
          this.itemForm.get('provinciaId')?.enable();
        }
        if (proveedor.provinciaId) {
          this.loadMunicipios(proveedor.provinciaId);
          this.itemForm.get('municipioId')?.enable();
        }
        if (proveedor.municipioId) {
          this.loadCodigosPostales(proveedor.municipioId);
          this.itemForm.get('codigoPostalId')?.enable();
        }

        this.itemForm.patchValue({
          ...proveedor,
          tipoIdentificadorFiscal: proveedor.tipoIdentificadorFiscal ?? null,
          regimenFiscal: proveedor.regimenFiscal ?? null,
          exentoIva: proveedor.exentoIva ?? false,
          extranjero: proveedor.extranjero ?? false,
          validarIdentificadorFiscal: proveedor.validarIdentificadorFiscal ?? false,
          inversionSujetoPasivo: proveedor.inversionSujetoPasivo ?? false
        });

        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error al cargar el proveedor';
        console.error(err);
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
    const formValue = this.itemForm.getRawValue();

    if (this.isEditMode && this.proveedorId) {
      this.proveedorService.update(this.proveedorId, formValue).subscribe({
        next: () => this.router.navigate(['/nomencladores/proveedor']),
        error: (err: any) => {
          this.error = 'Error al actualizar el proveedor';
          console.error(err);
          this.saving = false;
        }
      });
    } else {
      this.proveedorService.create(formValue).subscribe({
        next: () => this.router.navigate(['/nomencladores/proveedor']),
        error: (err: any) => {
          this.error = 'Error al crear el proveedor';
          console.error(err);
          this.saving = false;
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/nomencladores/proveedor']);
  }
}
