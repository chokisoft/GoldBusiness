import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { switchMap, catchError, skip } from 'rxjs/operators';
import { EstablecimientoService, EstablecimientoDTO, TipoEstablecimiento } from '../../../services/establecimiento.service';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';
import { TranslationService } from '../../../services/translation.service';
import { LanguageService } from '../../../services/language.service';
import { normalizePhone, phoneValidator, PHONE_MAX_LENGTH } from '../../shared/phone.util';

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
establecimientos: EstablecimientoDTO[] = []; // Para select de matriz
  
// Enum de tipos de establecimiento para el select
tiposEstablecimiento = Object.values(TipoEstablecimiento).filter(v => typeof v === 'number') as number[];

selectedPais?: PaisDTO;

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
    private translationService: TranslationService,
    private languageService: LanguageService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      // Campos básicos
      negocioId: [null, Validators.required],
      codigo: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      direccion: ['', Validators.maxLength(256)],
      telefono: ['', Validators.maxLength(PHONE_MAX_LENGTH)],
      
      // Información Organizacional
      tipo: [null, Validators.required],
      
      // Ubicación
      paisId: [null],
      provinciaId: [null],
      municipioId: [null],
      codigoPostalId: [null],
      
      // Estado
      operativoActualmente: [true],
      activo: [true],
      cancelado: [false]
    });

    this.itemForm.get('provinciaId')!.disable();
    this.itemForm.get('municipioId')!.disable();
    this.itemForm.get('codigoPostalId')!.disable();
  }

  ngOnInit(): void {
    this.loadPaises();
    this.loadNegocios();

    this.subs.push(
      this.languageService.currentLanguage$
        .pipe(skip(1))
        .subscribe(() => {
          this.loadPaises();
          this.loadNegocios();

          if (this.isEditMode && this.itemId) {
            this.loadItem();
            return;
          }

          const paisId = this.itemForm.get('paisId')?.value;
          const provinciaId = this.itemForm.get('provinciaId')?.value;
          const municipioId = this.itemForm.get('municipioId')?.value;

          if (paisId) {
            this.loadProvincias(Number(paisId));
          }

          if (provinciaId) {
            this.loadMunicipios(Number(provinciaId));
          }

          if (municipioId) {
            this.loadCodigoPostales(Number(municipioId));
          }
        })
    );

    this.subs.push(
      this.itemForm.get('paisId')!.valueChanges.pipe(
        switchMap((v: number | null) => {
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

    this.subs.push(this.itemForm.get('provinciaId')!.valueChanges.subscribe((v: number | null) => this.onProvinciaChange(v)));
    this.subs.push(this.itemForm.get('municipioId')!.valueChanges.subscribe((v: number | null) => this.onMunicipioChange(v)));

    this.route.params.subscribe((params: any) => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
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
      next: (list: SystemConfigurationDTO[]) => this.negocios = list,
      error: (err: any) => console.error('Error loading negocios', err)
    });
  }

  private loadPaises(): void {
    this.paisService.getAll().subscribe({
      next: (list: PaisDTO[]) => this.paises = list,
      error: (err: any) => console.error('Error loading paises', err)
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
      next: (list: ProvinciaDTO[]) => this.provincias = list,
      error: (err: any) => console.error('Error loading provincias', err)
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
      next: (list: MunicipioDTO[]) => this.municipios = list,
      error: (err: any) => console.error('Error loading municipios', err)
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
      next: (list: CodigoPostalDTO[]) => this.codigosPostales = list,
      error: (err: any) => console.error('Error loading codigos postales', err)
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
    
    // Aplicar validadores al teléfono principal
    const telefono = this.itemForm.get('telefono')!;
    const validators = [Validators.maxLength(PHONE_MAX_LENGTH)];

    if (pais && pais.regexTelefono) {
      validators.push(phoneValidator(pais.regexTelefono));
    }

    telefono.setValidators(validators);
    telefono.updateValueAndValidity({ emitEvent: false });
  }

  private loadItem(): void {
    if (!this.itemId) return;
    this.loading = true;
    this.error = null;

    this.establecimientoService.getById(this.itemId).subscribe({
      next: (item: EstablecimientoDTO) => {
        this.itemForm.patchValue({
          negocioId: item.negocioId,
          codigo: item.codigo,
          descripcion: item.descripcion,
          direccion: item.direccion,
          telefono: item.telefono,
          tipo: item.tipo,
          operativoActualmente: item.operativoActualmente,
          paisId: item.paisId,
          provinciaId: item.provinciaId,
          municipioId: item.municipioId,
          codigoPostalId: item.codigoPostalId,
          activo: item.activo,
          cancelado: item.cancelado
        }, { emitEvent: false });

        if (item.negocioId && !this.negocios.find((n: SystemConfigurationDTO) => n.id === item.negocioId)) {
          this.systemConfigurationService.getById(item.negocioId).subscribe({
            next: (n: SystemConfigurationDTO) => {
              if (n && !this.negocios.find((x: SystemConfigurationDTO) => x.id === n.id)) {
                this.negocios = [...this.negocios, n];
              }
            },
            error: (err: any) => console.error('Error loading negocio', err)
          });
        }

        if (item.paisId) {
          this.itemForm.patchValue({ paisId: item.paisId }, { emitEvent: false });

          this.paisService.getById(item.paisId).subscribe({
            next: (pais: PaisDTO) => this.applyPhoneValidators(pais),
            error: () => this.applyPhoneValidators(undefined)
          });

          this.provinciaService.getByPaisId(item.paisId).subscribe({
            next: (provinces: ProvinciaDTO[]) => {
              this.provincias = provinces;
              this.itemForm.get('provinciaId')!.enable();

              if (!item.provinciaId) {
                this.loading = false;
                return;
              }

              this.itemForm.patchValue({ provinciaId: item.provinciaId }, { emitEvent: false });

              this.municipioService.getByProvinciaId(item.provinciaId).subscribe({
                next: (municipios: MunicipioDTO[]) => {
                  this.municipios = municipios;
                  this.itemForm.get('municipioId')!.enable();

                  if (!item.municipioId) {
                    this.loading = false;
                    return;
                  }

                  this.itemForm.patchValue({ municipioId: item.municipioId }, { emitEvent: false });

                  this.codigoPostalService.getByMunicipioId(item.municipioId).subscribe({
                    next: (cps: CodigoPostalDTO[]) => {
                      this.codigosPostales = cps;
                      this.itemForm.get('codigoPostalId')!.enable();

                      if (item.codigoPostalId) {
                        this.itemForm.patchValue({ codigoPostalId: item.codigoPostalId }, { emitEvent: false });
                      }
                      this.loading = false;
                    },
                    error: (err: any) => {
                      console.error('Error loading cp', err);
                      this.loading = false;
                    }
                  });
                },
                error: (err: any) => {
                  console.error('Error loading municipios', err);
                  this.loading = false;
                }
              });
            },
            error: (err: any) => {
              console.error('Error loading provincias', err);
              this.loading = false;
            }
          });
        } else {
          this.applyPhoneValidators(undefined);
          this.loading = false;
        }
      },
      error: (err: any) => {
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

    const raw = this.itemForm.getRawValue();
    const formData: EstablecimientoDTO = {
      ...raw,
      id: this.itemId,
      telefono: normalizePhone(raw.telefono)
    };

    const request = this.isEditMode
      ? this.establecimientoService.update(this.itemId!, formData)
      : this.establecimientoService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/configuracion/establecimiento']);
      },
      error: (err: any) => {
        this.error = err.message || 'Error al guardar el establecimiento';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/configuracion/establecimiento']);
  }

  /**
   * Obtiene la etiqueta traducida para un tipo de establecimiento
   */
  getTipoEstablecimientoLabel(tipo: number): string {
    const keys: Record<number, string> = {
      [TipoEstablecimiento.SedeCentral]: 'establecimiento.tipo.sedeCentral',
      [TipoEstablecimiento.Sucursal]: 'establecimiento.tipo.sucursal',
      [TipoEstablecimiento.CentroDistribucion]: 'establecimiento.tipo.centroDistribucion',
      [TipoEstablecimiento.PlantaProduccion]: 'establecimiento.tipo.plantaProduccion',
      [TipoEstablecimiento.CentroServicios]: 'establecimiento.tipo.centroServicios',
      [TipoEstablecimiento.OficinaComercial]: 'establecimiento.tipo.oficinaComercial',
      [TipoEstablecimiento.CentroLogistico]: 'establecimiento.tipo.centroLogistico',
      [TipoEstablecimiento.PuntoAtencion]: 'establecimiento.tipo.puntoAtencion',
      [TipoEstablecimiento.Franquicia]: 'establecimiento.tipo.franquicia',
      [TipoEstablecimiento.OficinaAdministrativa]: 'establecimiento.tipo.oficinaAdministrativa'
    };
    return this.translationService?.translate(keys[tipo]) || '';
  }
}
