
#ifndef	__EFFEKSEER_ParameterNODE_MODEL_H__
#define	__EFFEKSEER_ParameterNODE_MODEL_H__

//----------------------------------------------------------------------------------
// Include
//----------------------------------------------------------------------------------
#include "Effekseer.EffectNode.h"

//----------------------------------------------------------------------------------
//
//----------------------------------------------------------------------------------
namespace Effekseer
{
//----------------------------------------------------------------------------------
//
//----------------------------------------------------------------------------------

//----------------------------------------------------------------------------------
//
//----------------------------------------------------------------------------------
class EffectNodeModel
	: public EffectNode
{
	friend class Manager;
	friend class Effect;
	friend class Instance;

public:

	struct InstanceValues
	{
		// �F
		color _color;

		union 
		{
			struct
			{
			
			} fixed;

			struct
			{

			} random;

			struct
			{
				color start;
				color  end;

			} easing;

			struct
			{
				float	offset[4];
			} fcurve_rgba;

		} allColorValues;
	};

public:
	eAlphaBlend		AlphaBlend;
	int32_t			ModelIndex;
	int32_t			NormalTextureIndex;
	bool			Lighting;
	eCullingType	Culling;

	StandardColorParameter	AllColor;

	EffectNodeModel( Effect* effect, unsigned char*& pos )
		: EffectNode( effect, pos )
	{
	}

	~EffectNodeModel()
	{
	}

	void LoadRendererParameter( unsigned char*& pos );

	void InitializeRenderer();

	void FinalizeRenderer();

	void BeginRendering( int32_t count );

	void Rendering( const Instance& instance );

	void EndRendering();

	void InitializeRenderedInstance( Instance& instance );

	void UpdateRenderedInstance( Instance& instance );

	eEffectNodeType GetType() const { return EFFECT_NODE_TYPE_MODEL; }
};

//----------------------------------------------------------------------------------
//
//----------------------------------------------------------------------------------
}
//----------------------------------------------------------------------------------
//
//----------------------------------------------------------------------------------
#endif	// __EFFEKSEER_ParameterNODE_MODEL_H__