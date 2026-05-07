import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { ClienteService } from '../../../services/cliente.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService } from '../../../services/provincia.service';
import { MunicipioService } from '../../../services/municipio.service';
import { CodigoPostalService } from '../../../services/codigo-postal.service';
import { TranslationService } from '../../../services/translation.service';
import { LanguageService } from '../../../services/language.service';
import { 
  FiscalOption,
  TipoIdentificacionFiscal,
  RegimenFiscal,
  getTipoIdentificacionFiscalOptions,
  getRegimenFiscalOptions
} from '../../../services/fiscal.types';

@Component({
  selector: 'app-cliente-form',
  templateUrl: './cliente-form.component.html',
  styleUrls: ['./cliente-form.component.css']
})
export class ClienteFormComponent implements OnInit, OnDestroy {
  itemForm!: FormGroup; // ⭐ RENOMBRADO de clienteForm
  clienteId?: number;
  isEditMode = false;
  
  error: string | null = null;
  saving = false;
  loading = false;
  loadingCuentas = false; // flag used by template when loading account data
  
  selectedPais?: PaisDTO; // ⭐ AGREGADO

  paises: any[] = [];
  provincias: any[] = [];
  municipios: any[] = [];
  codigosPostales: any[] = [];

  tiposIdentificacionFiscal: FiscalOption<TipoIdentificacionFiscal>[] = [];
  regimenesFiscales: FiscalOption<RegimenFiscal>[] = [];
  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private clienteService: ClienteService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private codigoPostalService: CodigoPostalService,
    private route: ActivatedRoute,
    private router: Router,
    public translationService: TranslationService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.refreshLocalizedOptions();
    this.loadPaises();

    this.clienteId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.clienteId) {
      this.isEditMode = true;
      this.loadCliente(this.clienteId);
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.refreshLocalizedOptions();
        this.reloadLocalizedData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private refreshLocalizedOptions(): void {
    this.tiposIdentificacionFiscal = getTipoIdentificacionFiscalOptions((key) => this.translationService.translate(key));
    this.regimenesFiscales = getRegimenFiscalOptions((key) => this.translationService.translate(key));
  }

  private reloadLocalizedData(): void {
    this.loadPaises();

    const paisId = this.itemForm.get('paisId')?.value;
    const provinciaId = this.itemForm.get('provinciaId')?.value;
    const municipioId = this.itemForm.get('municipioId')?.value;

    if (paisId) {
      this.loadProvincias(Number(paisId));
      this.loadPaisDetails(Number(paisId));
    }

    if (provinciaId) {
      this.loadMunicipios(Number(provinciaId));
    }

    if (municipioId) {
      this.loadCodigosPostales(Number(municipioId));
    }

    if (this.isEditMode && this.clienteId) {
      this.loadCliente(this.clienteId);
    }
  }

  private initForm(): void {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(8)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      identificadorFiscal: ['', Validators.maxLength(30)],
      iban: ['', Validators.maxLength(27)],
      bicoSwift: ['', Validators.maxLength(11)],
      tasaIva: [0, [Validators.required, Validators.min(-0.01), Validators.max(99.99)]],
      direccion: ['', Validators.maxLength(256)],
      telefono: ['', Validators.maxLength(50)],
      paisId: [null],
      provinciaId: [{ value: null, disabled: true }],
      municipioId: [{ value: null, disabled: true }],
      codigoPostalId: [{ value: null, disabled: true }],
      web: ['', Validators.maxLength(256)],
      email: ['', [Validators.email, Validators.maxLength(256)]],
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
        this.loadPaisDetails(paisId); // ⭐ CARGAR detalles del país
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

  private loadCliente(id: number): void {
    this.loading = true;
    this.clienteService.getById(id).subscribe({
      next: (cliente: any) => {
        if (cliente.paisId) {
          this.loadProvincias(cliente.paisId);
          this.loadPaisDetails(cliente.paisId);
          this.itemForm.get('provinciaId')?.enable();
        }
        if (cliente.provinciaId) {
          this.loadMunicipios(cliente.provinciaId);
          this.itemForm.get('municipioId')?.enable();
        }
        if (cliente.municipioId) {
          this.loadCodigosPostales(cliente.municipioId);
          this.itemForm.get('codigoPostalId')?.enable();
        }

        this.itemForm.patchValue({
          ...cliente,
          tipoIdentificadorFiscal: cliente.tipoIdentificadorFiscal ?? null,
          regimenFiscal: cliente.regimenFiscal ?? null,
          exentoIva: cliente.exentoIva ?? false,
          extranjero: cliente.extranjero ?? false,
          validarIdentificadorFiscal: cliente.validarIdentificadorFiscal ?? false,
          inversionSujetoPasivo: cliente.inversionSujetoPasivo ?? false
        });

        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error al cargar el cliente';
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

    if (this.isEditMode && this.clienteId) {
      this.clienteService.update(this.clienteId, formValue).subscribe({
        next: () => this.router.navigate(['/nomencladores/clientes']),
        error: (err: any) => {
          this.error = 'Error al actualizar el cliente';
          console.error(err);
          this.saving = false;
        }
      });
    } else {
      this.clienteService.create(formValue).subscribe({
        next: () => this.router.navigate(['/nomencladores/clientes']),
        error: (err: any) => {
          this.error = 'Error al crear el cliente';
          console.error(err);
          this.saving = false;
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/nomencladores/clientes']);
  }

  // Alias for templates using `cancel()` instead of `onCancel()`
  cancel(): void {
    this.onCancel();
  }

  // Compatibility alias: some templates reference `form` instead of `itemForm`
  get form(): FormGroup {
    return this.itemForm;
  }
}
