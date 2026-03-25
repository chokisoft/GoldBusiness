import * as fs from 'fs';
import * as path from 'path';
import * as glob from 'glob';

const root = process.cwd();
const clientTrans = path.join(root, 'GoldBusiness.Client', 'src', 'app', 'services', 'translation.service.ts');
if (!fs.existsSync(clientTrans)) {
  console.error('translation.service.ts not found at', clientTrans);
  process.exit(1);
}

const transText = fs.readFileSync(clientTrans, 'utf8');

// extract keys defined in translation.service.ts
const clientKeyRegex = /['"]([a-z0-9A-Z_.-]+)['"]\s*:\s*\{\s*['"]es['"]\s*:/g;
const clientKeys = new Set<string>();
let m: RegExpExecArray | null;
while ((m = clientKeyRegex.exec(transText)) !== null) clientKeys.add(m[1]);

// scan project files for translate usages
const patterns = [
  '**/*.html',
  '**/*.ts'
];
// gather used translation keys
const usedKeys = new Set<string>();
const translateCallRegex = /translate\(\s*['"]([^'"]+)['"]/g;
const pipeTranslateRegex = /['"]([^'"]+)['"]\s*\|\s*translate/g;
const doubleCurlyPipeRegex = /{{\s*['"]([^'"]+)['"]\s*\|\s*translate/g;

for (const pat of patterns) {
  const files = glob.sync(pat, { cwd: root, ignore: ['**/node_modules/**', '**/dist/**', '**/.git/**'] });
  for (const f of files) {
    const full = path.join(root, f);
    let content = '';
    try { content = fs.readFileSync(full, 'utf8'); } catch { continue; }
    let mm: RegExpExecArray | null;
    while ((mm = translateCallRegex.exec(content)) !== null) usedKeys.add(mm[1]);
    while ((mm = pipeTranslateRegex.exec(content)) !== null) usedKeys.add(mm[1]);
    while ((mm = doubleCurlyPipeRegex.exec(content)) !== null) usedKeys.add(mm[1]);
  }
}

// Filter used keys that look like placeholders/help (heuristic)
function looksLikePlaceholderOrHelp(k: string) {
  const lower = k.toLowerCase();
  return lower.includes('placeholder') || lower.includes('help') || lower.endsWith('.placeholder') || lower.endsWith('help') || lower.includes('codigoplaceholder') || lower.includes('descripcionplaceholder');
}

const usedPlaceholderHelp = Array.from(usedKeys).filter(looksLikePlaceholderOrHelp).sort();
const clientPlaceholderHelp = Array.from(clientKeys).filter(looksLikePlaceholderOrHelp).sort();

// compute differences
const missingInClient = usedPlaceholderHelp.filter(k => !clientKeys.has(k));
const clientOnly = clientPlaceholderHelp.filter(k => !usedKeys.has(k));

const report = {
  generatedAt: new Date().toISOString(),
  counts: {
    totalClientTranslationKeys: clientKeys.size,
    totalUsedTranslationKeys: usedKeys.size,
    usedPlaceholderHelpCount: usedPlaceholderHelp.length,
    clientPlaceholderHelpCount: clientPlaceholderHelp.length,
    missingInClient: missingInClient.length,
    clientOnly: clientOnly.length
  },
  missingInClient,
  clientOnly,
  sampleUsedPlaceholderHelp: usedPlaceholderHelp.slice(0,200),
  sampleClientPlaceholderHelp: clientPlaceholderHelp.slice(0,200)
};

if (!fs.existsSync(path.join(root, 'tools'))) fs.mkdirSync(path.join(root, 'tools'));
fs.writeFileSync(path.join(root, 'tools', 'placeholders-help-report.json'), JSON.stringify(report, null, 2), 'utf8');

console.log('Report saved to tools/placeholders-help-report.json');
console.log('missingInClient:', missingInClient.length, 'clientOnly:', clientOnly.length);