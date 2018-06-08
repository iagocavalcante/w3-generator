'use strict';
//Require dependencies
var Generator = require('yeoman-generator');
var chalk = require('chalk');
var yosay = require('yosay');
var fs = require('fs');
var mkdirp = require('mkdirp');
var path = require('path');
var guid = require('uuid');
const rename = require('gulp-rename');
var replace = require('gulp-replace');
var projectName = require('vs_projectname');
var pckg = require('../../package.json');

var W3Generator = class extends Generator {

  constructor(args, opts) {
    super(args, opts);

    this.argument('type', { type: String, required: false, desc: 'Tipo de projeto a ser criado' });
    this.argument('applicationName', { type: String, required: false, desc: 'Nome da aplicação' });

  }


  init() {
    this.log(yosay('Bem-vindo ao gerador de projetos com templates usados na w3!'));
    this.templatedata = {};
  }

  _checkProjectType() {
    if (this.options.type) {
      //normalize to lower case
      this.options.type = this.options.type.toLowerCase();
      var validProjectTypes = [
        'vue',
        'core',
        'spring',
        'node'
      ];

      if (validProjectTypes.indexOf(this.options.type) === -1) {
        //if it's not in the list, send them through the normal path
        this.log('"%s" não é um tipo de projeto válido', chalk.cyan(this.options.type));
        this.options.type = undefined;
        this.options.applicationName = undefined;
      } else {
        this.log('Criando projeto "%s"', chalk.cyan(this.options.type));
      }
    }
  }

  prompting() {
    this._checkProjectType();
    if (!this.options.type) {
      return this.prompt([
        {
          type: 'list',
          name: 'type',
          message: 'Qual o tipo de aplicação que você quer criar?',
          choices: [
            {
              name: 'Frontend com vue',
              value: 'vue'
            }, {
              name: 'API com .netcore 2',
              value: 'core'
            }, {
              name: 'API com spring',
              value: 'spring'
            }, {
              name: 'API com node',
              value: 'node'
            }
          ]
        },
        {
          name: 'applicationName',
          message: 'Qual o nome do seu projeto?',
        }])
        .then((answers) => {
          this.options.type = answers.type
          this.options.applicationName = answers.applicationName
          if (answers.type === 'spring') {
            return this.prompt([
              {
                name: 'package',
                message: 'Qual o nome do pacote?',
              }
            ]).then((answers) => { 
              this.options.package = answers.package                
            });
          }
        });
    }
  }

  _buildTemplateData() {
    this.templatedata.namespace = projectName(this.options.applicationName);
    this.templatedata.applicationname = this.options.applicationName;
    this.templatedata.includeApplicationInsights = false;
    this.templatedata.guid = guid.v4();
    this.templatedata.sqlite = (this.options.type === 'mvc') ? true : false;
    this.templatedata.ui = this.ui;
    this.templatedata.version = "1.0.0-rc4-004771";
    this.templatedata.dotnet = {
      version: this.options['versionCurrent'] ?
        pckg.dotnet.current.version : pckg.dotnet.lts.version,
      targetFramework: this.options['versionCurrent'] ?
        pckg.dotnet.current.targetFramework : pckg.dotnet.lts.targetFramework
    };
  }

  askForName() {
    if (!this.options.applicationName) {
      var app = '';
      switch (this.options.type) {
        case 'vue':
          app = 'BlankVueProject';
          break;
        case 'core':
          app = 'BlankAspNetCoreProject';
          break;
        case 'spring':
          app = 'BlankSpringProject';
          break;
        case 'node':
          app = 'BlankNodeProject';
          break;
      }
      var prompts = [{
        name: 'applicationName',
        message: this.options.type !== 'core' ? 'Qual o nome do seu projeto?' : 'Qual o nome da sua solução?',
        default: app
      }];
      return this.prompt(prompts)
        .then((answers) => {
          this.options.applicationName = answers.applicationName;
          this._buildTemplateData();
        })
    } else {
      this._buildTemplateData();
    }
  }

  writing() {
    this.sourceRoot(path.join(__dirname, './templates/projects'));
    switch (this.options.type) {

      case 'vue':
        this.sourceRoot(path.join(__dirname, '../templates/projects/' + this.options.type));

        this.copy(this.sourceRoot() + '/../../gitignore.txt', this.options.applicationName + '/.gitignore');

        this.template(this.sourceRoot() + '/Program.cs', this.options.applicationName + '/Program.cs', this.templatedata);

        this.template(this.sourceRoot() + '/Startup.cs', this.options.applicationName + '/Startup.cs', this.templatedata);

        this.template(this.sourceRoot() + '/Company.WebApplication1.csproj', this.options.applicationName + '/' + this.applicationName + '.csproj', this.templatedata);

        this.copy(this.sourceRoot() + '/web.config', this.options.applicationName + '/web.config');

        /// Properties
        this.fs.copyTpl(this.templatePath('Properties/**/*'), this.options.applicationName + '/Properties', this.templatedata);
        this.copy(this.sourceRoot() + '/runtimeconfig.template.json', this.options.applicationName + '/runtimeconfig.template.json');
        this.fs.copy(this.sourceRoot() + '/README.md', this.options.applicationName + '/README.md');
        mkdirp.sync(this.options.applicationName + '/wwwroot');
        this.template(this.sourceRoot() + '/../../global.json', this.options.applicationName + '/global.json', this.templatedata);
        break;

      case 'core':
        this.sourceRoot(path.join(__dirname, '../app/templates/' + this.options.type));

        let solutionName = this.options.applicationName;
        
        this.registerTransformStream(rename(function (path) {
          path.basename = path.basename.replace(/(APIModelo)/g, solutionName);
          path.dirname = path.dirname.replace(/(APIModelo)/g, solutionName);
          return path;
        }));
        
        this.fs.copyTpl(
          this.templatePath(),
          this.options.applicationName,
          {
            solutionName: solutionName,
          }
        );

        this.fs.copy(this.sourceRoot() + '/gitignore.txt', this.options.applicationName + '/.gitignore');

        break;

      case 'spring':
        this.sourceRoot(path.join(__dirname, '../app/templates/' + this.options.type));

        let nomeProjeto = this.options.applicationName;
        let pacote = this.options.package;

        this.registerTransformStream(rename(function (path) {
          path.basename = path.basename.replace(/(nomeProjeto)/g, nomeProjeto);
          path.dirname = path.dirname.replace(/(nomeProjeto)/g, nomeProjeto);
          
          path.basename = path.basename.replace(/(pacote)/g, pacote);
          path.dirname = path.dirname.replace(/(pacote)/g, pacote);
          return path;
        }));

        this.fs.copyTpl(
          this.templatePath(),
          this.options.applicationName,
          {
            nomeProjeto: nomeProjeto,
            pacote: pacote,
          }
        );
        break;

      case 'node':
        this.sourceRoot(path.join(__dirname, '../app/templates/' + this.options.type));

        let projectName = this.options.applicationName;

        this.registerTransformStream(rename(function (path) {
          path.basename = path.basename.replace(/(node)/g, projectName);
          path.dirname = path.dirname.replace(/(node)/g, projectName);
          return path;
        }));

        this.fs.copy(
          this.templatePath(),
          this.options.applicationName
        );

        break;
    }
  }

  end() {
    switch (this.options.type) {
      case 'core':
        this.log('\r\n');
        this.log('Seu projeto foi criado, você pode usar os comandos a seguir para continuar!');
        this.log(chalk.green('    cd "' + this.options.applicationName + '"'));
        this.log(chalk.green('    dotnet restore'));
        this.log(chalk.green('    dotnet build') + ' (opcional)');
        this.log('\r\n');
        
        break;
      case 'node':
        this.log('\r\n');
        this.log('Seu projeto foi criado, você pode usar os comandos a seguir para continuar!');
        this.log(chalk.green('    cd "' + this.options.applicationName + '"'));
        this.log(chalk.green('    npm install'));
        this.log('configure o projeto adequadamente!');
        this.log(chalk.green('    npm run dev'));
        this.log('\r\n');
        break;
    }
  }

}

module.exports = W3Generator;