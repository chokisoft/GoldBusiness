import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators';
import { EstablecimientoService, EstablecimientoDTO } from '../../../services/establecimiento.service';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';

@Component({
  selector: 'app-establecimiento-form',
  templateUrl: './establecimiento-form.component.html',
  styleUrls: ['./establecimiento-form.component.css']
})
export class EstablecimientoFormComponent implements OnInit, OnDestroy {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;

  paises: PaisDTO[] = [];
  provincias: ProvinciaDTO[] = [];
  municipios: MunicipioDTO[] = [];
  codigosPostales: CodigoPostalDTO[] = [];
  negocios: SystemConfigurationDTO[] = [];

  selectedPais?: PaisDTO;

  // Small helpers so shared templates that expect these names work unchanged
  loadingSubGrupos = false;
  loadingGrupos = false;
  get form(): FormGroup { return this.itemForm; }
  cancel(): void { this.onCancel(); }

  private subs: Subscription[] = [];

  constructor(
    private fb: FormBuilder,
    private establecimientoService: EstablecimientoService,
    private systemConfigurationService: SystemConfigurationService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private codigoPostalService: CodigoPostalService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      negocioId: [null, Validators.required],
      codigo: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      direccion: ['', Validators.maxLength(256)],
      telefono: ['', Validators.maxLength(50)],
      paisId: [null],
      provinciaId: [null],
      municipioId: [null],
      codigoPostalId: [null],
      activo: [true],
      cancelado: [false]
    });

    // Inicialmente deshabilitar dependientes
    this.itemForm.get('provinciaId')!.disable();
    this.itemForm.get('municipioId')!.disable();
    this.itemForm.get('codigoPostalId')!.disable();
  }

  ngOnInit(): void {
    this.loadPaises();
    this.loadNegocios();

    this.subs.push(
      this.itemForm.get('paisId')!.valueChanges.pipe(
        switchMap(v => {
          this.onPaisChange(v);
          if (v) {
            return this.paisService.getById(+v).pipe(catchError(() => of(null)));
          }
          return of(null);
        })
      ).subscribe((pais: PaisDTO | null) => {
        this.applyPhoneValidators(pais || undefined);
      })
    );

    this.subs.push(this.itemForm.get('provinciaId')!.valueChanges.subscribe(v => this.onProvinciaChange(v)));
    this.subs.push(this.itemForm.get('municipioId')!.valueChanges.subscribe(v => this.onMunicipioChange(v)));

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
        // Deshabilitar negocioId y codigo en modo edición
        this.itemForm.get('negocioId')?.disable();
        this.itemForm.get('codigo')?.disable();
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  private loadNegocios(): void {
    this.systemConfigurationService.getAll().subscribe({
      next: list => this.negocios = list,
      error: err => console.error('Error loading negocios (system configurations)', err)
    });
  }

  private loadPaises(): void {
    this.paisService.getAll().subscribe({
      next: list => this.paises = list,
      error: err => console.error('Error loading paises', err)
    });
  }

  private loadProvincias(paisId?: number): void {
    this.provincias = [];
    this.municipios = [];
    this.codigosPostales = [];
    this.itemForm.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });
    this.itemForm.get('municipioId')!.disable();
    this.itemForm.get('codigoPostalId')!.disable();

    if (!paisId) {
      this.itemForm.get('provinciaId')!.disable();
      return;
    }

    this.itemForm.get('provinciaId')!.enable();
    this.provinciaService.getByPaisId(paisId).subscribe({
      next: list => this.provincias = list,
      error: err => console.error('Error loading provincias', err)
    });
  }

  private loadMunicipios(provinciaId?: number): void {
    this.municipios = [];
    this.codigosPostales = [];
    this.itemForm.patchValue({ municipioId: null, codigoPostalId: null }, { emitEvent: false });
    this.itemForm.get('codigoPostalId')!.disable();

    if (!provinciaId) {
      this.itemForm.get('municipioId')!.disable();
      return;
    }

    this.itemForm.get('municipioId')!.enable();
    this.municipioService.getByProvinciaId(provinciaId).subscribe({
      next: list => this.municipios = list,
      error: err => console.error('Error loading municipios', err)
    });
  }

  private loadCodigoPostales(municipioId?: number): void {
    this.codigosPostales = [];
    this.itemForm.patchValue({ codigoPostalId: null }, { emitEvent: false });

    if (!municipioId) {
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }

    this.itemForm.get('codigoPostalId')!.enable();
    this.codigoPostalService.getByMunicipioId(municipioId).subscribe({
      next: list => this.codigosPostales = list,
      error: err => console.error('Error loading codigos postales', err)
    });
  }

  onPaisChange(value: number | null): void {
    const paisId = value ? +value : undefined;
    if (!paisId) {
      this.selectedPais = undefined;
      this.itemForm.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });
      this.provincias = [];
      this.municipios = [];
      this.codigosPostales = [];
      this.itemForm.get('provinciaId')!.disable();
      this.itemForm.get('municipioId')!.disable();
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadProvincias(paisId);
  }

  onProvinciaChange(value: number | null): void {
    const provinciaId = value ? +value : undefined;
    if (!provinciaId) {
      this.itemForm.patchValue({ municipioId: null, codigoPostalId: null }, { emitEvent: false });
      this.municipios = [];
      this.codigosPostales = [];
      this.itemForm.get('municipioId')!.disable();
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadMunicipios(provinciaId);
  }

  onMunicipioChange(value: number | null): void {
    const municipioId = value ? +value : undefined;
    if (!municipioId) {
      this.itemForm.patchValue({ codigoPostalId: null }, { emitEvent: false });
      this.codigosPostales = [];
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadCodigoPostales(municipioId);
  }

  private applyPhoneValidators(pais?: PaisDTO): void {
    this.selectedPais = pais;

    const telefono = this.itemForm.get('telefono')!;
    const baseValidators = [Validators.maxLength(50)];

    if (pais && pais.regexTelefono) {
      try {
        const re = new RegExp(pais.regexTelefono);
        const phonePatternValidator: ValidatorFn = (c: AbstractControl) => {
          if (!c.value) return null;
          return re.test(c.value) ? null : { telefonoInvalid: true };
        };
        telefono.setValidators([...baseValidators, phonePatternValidator]);
      } catch {
        telefono.setValidators(baseValidators);
        console.warn('Invalid regexTelefono from API for pais', pais?.id);
      }
    } else {
      telefono.setValidators(baseValidators);
    }

    telefono.updateValueAndValidity({ emitEvent: false });
  }

  private loadItem(): void {
    if (!this.itemId) return;
    this.loading = true;
    this.error = null;

    this.establecimientoService.getById(this.itemId).subscribe({
      next: item => {
        this.itemForm.patchValue({
          negocioId: item.negocioId,
          codigo: item.codigo,
          descripcion: item.descripcion,
          direccion: item.direccion,
          telefono: item.telefono,
          activo: item.activo,
          cancelado: item.cancelado
        }, { emitEvent: false });

        // Si el negocio asociado no está en la lista, cargarlo y añadirlo
        if (item.negocioId && !this.negocios.find(n => n.id === item.negocioId)) {
          this.systemConfigurationService.getById(item.negocioId).subscribe({
            next: n => {
              if (n && !this.negocios.find(x => x.id === n.id)) {
                this.negocios = [...this.negocios, n];
              }
            },
            error: err => console.error('Error loading negocio (system configuration)', err)
          });
        }

        if (item.paisId) {
          this.itemForm.patchValue({ paisId: item.paisId }, { emitEvent: false });

          this.paisService.getById(item.paisId).subscribe({
            next: pais => this.applyPhoneValidators(pais),
            error: () => this.applyPhoneValidators(undefined)
          });

          this.provinciaService.getByPaisId(item.paisId).subscribe({
            next: provinces => {
              this.provincias = provinces;
              this.itemForm.get('provinciaId')!.enable();

              if (!item.provinciaId) {
                this.loading = false;
                return;
              }

              this.itemForm.patchValue({ provinciaId: item.provinciaId }, { emitEvent: false });

              this.municipioService.getByProvinciaId(item.provinciaId).subscribe({
                next: municipios => {
                  this.municipios = municipios;
                  this.itemForm.get('municipioId')!.enable();

                  if (!item.municipioId) {
                    this.loading = false;
                    return;
                  }

                  this.itemForm.patchValue({ municipioId: item.municipioId }, { emitEvent: false });

                  this.codigoPostalService.getByMunicipioId(item.municipioId).subscribe({
                    next: cps => {
                      this.codigosPostales = cps;
                      this.itemForm.get('codigoPostalId')!.enable();

                      if (item.codigoPostalId) {
                        this.itemForm.patchValue({ codigoPostalId: item.codigoPostalId }, { emitEvent: false });
                      }
                      this.loading = false;
                    },
                    error: err => {
                      console.error('Error loading cp', err);
                      this.loading = false;
                    }
                  });
                },
                error: err => {
                  console.error('Error loading municipios', err);
                  this.loading = false;
                }
              });
            },
            error: err => {
              console.error('Error loading provincias', err);
              this.loading = false;
            }
          });
        } else {
          this.applyPhoneValidators(undefined);
          this.loading = false;
        }
      },
      error: err => {
        this.error = err.message || 'Error al cargar el establecimiento';
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

    // Usar getRawValue() para incluir controles deshabilitados (codigo, negocioId en edición)
    const raw = this.itemForm.getRawValue();
    const formData: EstablecimientoDTO = {
      ...raw,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.establecimientoService.update(this.itemId!, formData)
      : this.establecimientoService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/nomencladores/establecimiento']);
      },
      error: err => {
        this.error = err.message || 'Error al guardar el establecimiento';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/nomencladores/establecimiento']);
  }
}
